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
        class MsgInfo
        {
            public int msgID;
            public int tick;
            public bool isSignaled;
            public string time;
            public string user;
            public string message;
        }

        int mRoomID;
        int mCntLoingUser;
        Dictionary<string, int> mUsers = new Dictionary<string, int>();//int값은 User의 현재 메세지 위치를 기억함
        List<MsgInfo> mMessages = new List<MsgInfo>();

        public void Init(int chatID)
        {
            mRoomID = chatID;
            mCntLoingUser = 0;

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
                mUsers[user] = cntMessages;
            }
        }
        public void ProcChat(Chat obj)
        {
            switch(obj.msgID)
            {
                case DEF.CMD_ChatMsg:
                    ProcMessage(obj);
                    break;
                case DEF.CMD_AddChatUser:
                    ProcNewUser(obj);
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
                default:
                    LOG.warn();
                    break;
            }
        }


        private void BroadcastChatUsers()
        {
            Chat msg = new ICD.Chat();
            msg.FillServerHeader(DEF.CMD_ChatUserList);
            msg.recordID = mRoomID;
            foreach (var user in mUsers)
            {
                msg.info += String.Format("%s\0",user.Key);
            }

            foreach (var user in mUsers)
            {
                MsgCtrl.GetInst().sendMsg(user.Key, msg);
            }
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
                    string chatInfo = String.Format("%d,%d,%s,%s,%s\0",
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

            foreach(var user in mUsers)
            {
                MsgCtrl.GetInst().sendMsg(user.Key, msg);
            }
        }
        private void ProcMessage(Chat obj)
        {
            int msgID = DatabaseMgr.PushChatMessage(obj);
            MsgInfo item = new MsgInfo();
            item.msgID = msgID;
            item.time = obj.msgTime;
            item.user = obj.msgUser;
            item.tick = mUsers.Count - mCntLoingUser;
            if (item.tick < 0)
                item.tick = 0;
            item.message = obj.info.Substring(0, obj.info.Length);
            item.isSignaled = true;
            mMessages.Add(item);
            BroadcastSignaledMsgs();
        }
        private void ProcNewUser(Chat obj)
        {
            mUsers[obj.info] = mMessages.Count;
            BroadcastChatUsers();
        }
        private void ProcDelUser(Chat obj)
        {
            mUsers[obj.info] = -2;
            mUsers.Remove(obj.info);
            BroadcastChatUsers();
        }
        private void ProcInUser(Chat obj)
        {
            mCntLoingUser++;
            int curCount = mUsers[obj.msgUser];
            mUsers[obj.msgUser] = -1;
            int totalCount = mMessages.Count;
            for (int i=curCount; i< totalCount; ++i)
            {
                mMessages[i].isSignaled = true;
                mMessages[i].tick = (mMessages[i].tick > 0) ? mMessages[i].tick - 1 : 0;
            }
            BroadcastSignaledMsgs();
        }
        private void ProcOutUser(Chat obj)
        {
            if(mCntLoingUser>0)
                mCntLoingUser--;

            mUsers[obj.msgUser] = mMessages.Count;

        }
    }
}
