using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IWAS.ICD;
using System.Data;

namespace IWAS
{
    class ChatRoom
    {
        public class MsgInfo
        {
            public int msgID;
            public int tick;
            public int index;
            public bool isSignaled;
            public string time;
            public string user;
            public string message;
        }

        private int mRoomID;
        Dictionary<string, int> mUsers = new Dictionary<string, int>();//int값은 User의 현재 메세지 위치를 기억함
        List<MsgInfo> mMessages = new List<MsgInfo>();

        public void Init(int chatID)
        {
            mRoomID = chatID;

            DataTable rowMsgs = DatabaseMgr.GetChatMessages(mRoomID);
            if(rowMsgs != null)
            {
                foreach (DataRow item in rowMsgs.Rows)
                {
                    MsgInfo info = new MsgInfo();
                    string msg = item["info"].ToString();
                    info.message = msg.Substring(0, msg.Length);
                    info.isSignaled = false;
                    info.msgID = (int)item["recordID"];
                    info.time = item["time"].ToString();
                    info.user = item["user"].ToString();
                    info.tick = 0;
                    mMessages.Add(info);
                }
            }

            int cntMessages = mMessages.Count;
            DataRow room = DatabaseMgr.GetChatRoomInfo(mRoomID);
            if (room == null)
                return;

            string visitors = room["visitors"].ToString();
            string[] users = visitors.Split(',');
            foreach (string user in users)
            {
                if(user.Length > 0)
                    mUsers[user] = cntMessages;
            }

        }
        public void ProcChat(Chat obj)
        {
            if( !mUsers.ContainsKey(obj.msgUser) )
            {
                LOG.warn();
                return;
            }

            switch(obj.msgID)
            {
                case DEF.CMD_ChatMsg:
                    ProcMessage(obj);
                    break;
                case DEF.CMD_ChatMsgAll:
                    ProcMessageAll(obj);
                    break;
                case DEF.CMD_SetChatUsers:
                    ProcSetUsers(obj);
                    break;
                case DEF.CMD_DelChatUser:
                    ProcDelUser(obj);
                    break;
                case DEF.CMD_ShowChat:
                    ProcInUser(obj);
                    break;
                case DEF.CMD_HideChat:
                    ProcOutUser(obj);
                    break;
                case DEF.CMD_ChatRoomInfo:
                    ProcUserList(obj);
                    break;
                default:
                    LOG.warn();
                    break;
            }
        }

        public int ProcNewChat(ChatRoomList obj)
        {
            DataRow row = DatabaseMgr.PushNewChat(obj);
            mRoomID = (int)row["recordID"];
            mMessages.Clear();

            foreach (var user in obj.body[0].users)
            {
                if (user.Length > 0)
                    mUsers[user] = 0;
            }

            obj.body[0].recordID = mRoomID;
            DatabaseMgr.EditChatUsers(obj);
            BroadcastRoomInfo();

            return mRoomID;
        }

        public int ProcNewChat(Chat obj)
        {
            DataRow row = DatabaseMgr.PushNewChat(obj);
            mRoomID = (int)row["recordID"];
            mMessages.Clear();

            string[] newUsers = obj.info.Split(',');
            foreach (var user in newUsers)
            {
                if (user.Length > 0)
                    mUsers[user] = 0;
            }

            obj.recordID = mRoomID;
            DatabaseMgr.EditChatUsers(obj);
            BroadcastChatUsers();

            return mRoomID;
        }
        private void ProcUserList(Chat obj)
        {
            Chat msg = new Chat();
            msg.FillServerHeader(DEF.CMD_ChatRoomInfo);
            msg.recordID = mRoomID;
            msg.state = GetUserState(obj.msgUser);
            foreach (var user in mUsers)
                msg.info += String.Format("{0},", user.Key);

            MsgCtrl.GetInst().sendMsg(obj.msgUser, msg);
        }

        private void BroadcastRoomInfo()
        {
            ChatRoomList msg = new ICD.ChatRoomList();
            msg.FillServerHeader(DEF.CMD_ChatRoomInfo, 0);
            msg.body[0].recordID = mRoomID;
            msg.body[0].users = new string[mUsers.Count];
            for (int i=0; i<mUsers.Count; ++i)
            {
                msg.body[0].users[i] = mUsers.ElementAt(i).Key;
            }

            foreach (var user in mUsers)
            {
                msg.body[0].state = GetUserState(user.Key);
                MsgCtrl.GetInst().sendMsg(user.Key, msg);
            }

        }

        private void BroadcastChatUsers()
        {
            Chat msg = new ICD.Chat();
            msg.FillServerHeader(DEF.CMD_ChatRoomInfo);
            msg.recordID = mRoomID;
            foreach (var user in mUsers)
                msg.info += String.Format("{0},",user.Key);

            foreach (var user in mUsers)
            {
                msg.state = GetUserState(user.Key);
                MsgCtrl.GetInst().sendMsg(user.Key, msg);
            }
        }

        private int CountLoginUser()
        {
            int ret = 0;
            foreach(var item in mUsers)
            {
                if (item.Value == -1)
                    ret++;
            }
            return ret;
        }

        private void BroadcastSignaledMsgs()
        {
            Chat msg = new ICD.Chat();
            msg.FillServerHeader(DEF.CMD_ChatMsgList);
            msg.recordID = mRoomID;
            msg.info = "";
            int cnt = mMessages.Count;
            for(int i=cnt-1; i>=0; --i)
            {
                if (mMessages[i].isSignaled)
                {
                    string chatInfo = String.Format("{0},{1},{2},{3},{4}\\",
                        mMessages[i].msgID,
                        mMessages[i].tick,
                        mMessages[i].time,
                        mMessages[i].user,
                        mMessages[i].message);

                    msg.info += chatInfo;
                    mMessages[i].isSignaled = false;
                }
                else
                    break;
            }

            if(msg.info.Length >= 260)
            {
                LOG.warn();
                return;
            }

            int msgCount = mMessages.Count;
            foreach(var user in mUsers)
            {
                if(user.Value == -1) //user가 채팅방 창을 보고있는 상태
                {
                    msg.msgID = DEF.CMD_ChatMsgList;
                    msg.state = 0;
                    MsgCtrl.GetInst().sendMsg(user.Key, msg);
                }
                else//user가 채팅방 창을 안보는 상태는 알람 message 전송
                {
                    msg.msgID = DEF.CMD_AlarmChat;
                    msg.state = msgCount - user.Value;
                    MsgCtrl.GetInst().sendMsg(user.Key, msg);
                }
                
            }
        }

        private void ProcMessageAll(Chat obj)
        {
            Chat msg = new ICD.Chat();
            msg.FillServerHeader(DEF.CMD_ChatMsgList);
            msg.recordID = mRoomID;

            foreach (var item in mMessages)
            {
                string chatInfo = String.Format("{0},{1},{2},{3},{4}\0",
                    item.msgID,
                    item.tick,
                    item.time,
                    item.user,
                    item.message);

                msg.info = chatInfo;

                MsgCtrl.GetInst().sendMsg(obj.msgUser, msg);
            }
        }

        private void ProcMessage(Chat obj)
        {
            DataRow row = DatabaseMgr.PushChatMessage(obj);
            int msgID = (int)row["recordID"];
            MsgInfo item = new MsgInfo();
            item.msgID = msgID;
            item.time = obj.msgTime;
            item.user = obj.msgUser;
            item.tick = mUsers.Count - CountLoginUser();
            item.message = obj.info.Substring(0, obj.info.Length);
            item.isSignaled = true;
            mMessages.Add(item);
            BroadcastSignaledMsgs();
        }
        private void ProcDelUser(Chat obj)
        {
            if (mUsers.ContainsKey(obj.info))
            {
                mUsers.Remove(obj.info);

                Chat tmp = new Chat();
                tmp.recordID = mRoomID;
                tmp.info = GetUserList();
                DatabaseMgr.EditChatUsers(tmp);

                BroadcastChatUsers();
            }
        }
        private void ProcSetUsers(Chat obj)
        {
            foreach(var item in mUsers)
            {
                if( !obj.info.Contains(item.Key) )
                {
                    Chat msg = new Chat();
                    msg.FillServerHeader(DEF.CMD_DelChatUser);
                    msg.recordID = mRoomID;
                    MsgCtrl.GetInst().sendMsg(item.Key, msg);

                    mUsers.Remove(item.Key);
                }
            }

            string[] newUsers = obj.info.Split(',');
            foreach (var user in newUsers)
            {
                if (user.Length > 0 && !mUsers.ContainsKey(user))
                    mUsers[user] = mMessages.Count;
            }

            DatabaseMgr.EditChatUsers(obj);
            BroadcastChatUsers();
        }
        private void ProcInUser(Chat obj)
        {
            int curCount = mUsers[obj.msgUser];
            mUsers[obj.msgUser] = -1;
            int totalCount = mMessages.Count;
            for (int i=curCount; i< totalCount; ++i)
            {
                mMessages[i].isSignaled = true;
                mMessages[i].tick = (mMessages[i].tick > 0) ? mMessages[i].tick - 1 : 0 ;
            }
            BroadcastSignaledMsgs();
        }
        private void ProcOutUser(Chat obj)
        {
            if(mUsers.ContainsKey(obj.msgUser))
            {
                mUsers[obj.msgUser] = mMessages.Count;
            }
        }
        public int GetUserState(string user)
        {
            if( !mUsers.ContainsKey(user) )
                return -1;

            return mMessages.Count - mUsers[user];
        }
        public string GetUserList()
        {
            string list = "";
            foreach(var item in mUsers)
            {
                list += item.Key;
                list += ",";
            }
            return list;
        }
    }
}
