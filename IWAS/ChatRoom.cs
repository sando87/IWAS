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
            if (rowMsgs != null)
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
                if (user.Length > 0)
                    mUsers[user] = cntMessages;
            }

        }
        public void ProcChat(ChatRoomInfo obj)
        {
            if (!mUsers.ContainsKey(obj.msgUser))
            {
                LOG.warn();
                return;
            }

            switch (obj.msgID)
            {
                case DEF.CMD_ChatMsg:
                    ProcMessage(obj);
                    break;
                case DEF.CMD_ChatMsgAll:
                    ProcMessageAll(obj);
                    break;
                case DEF.CMD_AddChatUsers:
                    ProcAddUsers(obj);
                    break;
                case DEF.CMD_DelChatUsers:
                    ProcDelUser(obj);
                    break;
                case DEF.CMD_ShowChat:
                    ProcInUser(obj);
                    break;
                case DEF.CMD_HideChat:
                    ProcOutUser(obj);
                    break;
                case DEF.CMD_ChatRoomInfo:
                    ProcRoomInfo(obj);
                    break;
                default:
                    LOG.warn();
                    break;
            }
        }

        public int ProcNewChat(ChatRoomInfo obj)
        {
            DataRow row = DatabaseMgr.PushNewChat(obj);
            mRoomID = (int)row["recordID"];
            mMessages.Clear();

            foreach (var user in obj.body.users)
            {
                mUsers[user] = 0;
            }

            ChatRoomInfo msg = GetRoomInfo();
            DatabaseMgr.EditChatUsers(msg);
            BroadcastRoomInfo(msg);

            return mRoomID;
        }

        private void ProcRoomInfo(ChatRoomInfo obj)
        {
            ChatRoomInfo msg = GetRoomInfo();
            msg.body.state = GetUserState(obj.msgUser);
            MsgCtrl.GetInst().sendMsg(obj.msgUser, msg);
        }

        public ChatRoomInfo GetRoomInfo(bool isAll = false)
        {
            ChatRoomInfo msg = new ChatRoomInfo();
            msg.FillServerHeader(DEF.CMD_ChatRoomInfo, 0);

            RoomInfo room = msg.body;
            room.recordID = mRoomID;
            room.users = new string[mUsers.Count];
            for (int i = 0; i < mUsers.Count; ++i)
            {
                room.users[i] = mUsers.ElementAt(i).Key;
            }

            List<MesgInfo> vecMsgs = new List<MesgInfo>();
            int cnt = mMessages.Count;
            for (int i = cnt - 1; i >= 0; --i)
            {
                if (!isAll && !mMessages[i].isSignaled)
                    break;

                MesgInfo mesg = new MesgInfo();
                mesg.recordID = mMessages[i].msgID;
                mesg.tick = mMessages[i].tick;
                mesg.time = mMessages[i].time;
                mesg.user = mMessages[i].user;
                mesg.message = mMessages[i].message;

                vecMsgs.Add(mesg);
                mMessages[i].isSignaled = false;
            }

            room.mesgs = vecMsgs.ToArray();
            return msg;
        }

        private void BroadcastRoomInfo(ChatRoomInfo msg)
        {
            foreach (var user in mUsers)
            {
                msg.body.state = GetUserState(user.Key);
                MsgCtrl.GetInst().sendMsg(user.Key, msg);
            }
        }

        private int CountLoginUser()
        {
            int ret = 0;
            foreach (var item in mUsers)
            {
                if (item.Value == -1)
                    ret++;
            }
            return ret;
        }


        private void ProcMessageAll(ChatRoomInfo obj)
        {
            ChatRoomInfo msg = GetRoomInfo(true);
            msg.body.state = GetUserState(obj.msgUser);
            MsgCtrl.GetInst().sendMsg(obj.msgUser, msg);
        }

        private void ProcMessage(ChatRoomInfo obj)
        {
            DataRow row = DatabaseMgr.PushChatMessage(obj);
            int msgID = (int)row["recordID"];
            MsgInfo item = new MsgInfo();
            item.msgID = msgID;
            item.time = obj.msgTime;
            item.user = obj.msgUser;
            item.tick = mUsers.Count - CountLoginUser();
            item.message = obj.body.mesgs[0].message;
            item.isSignaled = true;
            mMessages.Add(item);

            ChatRoomInfo msg = GetRoomInfo();
            BroadcastRoomInfo(msg);
        }

        private void UpdateTick(string user)
        {
            int curCount = mUsers[user];
            if(curCount < 0)
                return;

            mUsers[user] = -1;
            int totalCount = mMessages.Count;
            for (int i = curCount; i < totalCount; ++i)
            {
                mMessages[i].isSignaled = true;
                mMessages[i].tick = (mMessages[i].tick > 0) ? mMessages[i].tick - 1 : 0;
            }
        }

        private void ProcDelUser(ChatRoomInfo obj)
        {
            foreach(string name in obj.body.users)
            {
                if (!mUsers.ContainsKey(name))
                    continue;

                UpdateTick(name);
                mUsers.Remove(name);

                ChatRoomInfo msg = new ChatRoomInfo();
                msg.FillServerHeader(DEF.CMD_DelChatUsers, 0);
                msg.body.recordID = mRoomID;
                MsgCtrl.GetInst().sendMsg(name, msg);
            }

            ChatRoomInfo tmp = GetRoomInfo();
            DatabaseMgr.EditChatUsers(tmp);
            BroadcastRoomInfo(tmp);
        }

        private void ProcAddUsers(ChatRoomInfo obj)
        {
            foreach (string name in obj.body.users)
            {
                if (mUsers.ContainsKey(name))
                    continue;

                mUsers[name] = mMessages.Count;
            }

            ChatRoomInfo tmp = GetRoomInfo();
            DatabaseMgr.EditChatUsers(tmp);
            BroadcastRoomInfo(tmp);
        }

        private void ProcInUser(ChatRoomInfo obj)
        {
            UpdateTick(obj.msgUser);
            ChatRoomInfo msg = GetRoomInfo();
            BroadcastRoomInfo(msg);
        }

        private void ProcOutUser(ChatRoomInfo obj)
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
