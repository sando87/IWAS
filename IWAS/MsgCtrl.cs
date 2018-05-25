using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWAS.ICD;

namespace IWAS
{
    class MsgCtrl
    {
        static private MsgCtrl mInst = null;
        static public MsgCtrl GetInst() { if (mInst == null) mInst = new MsgCtrl(); return mInst; }
        private MsgCtrl() { }

        ICDPacketMgr.PacketHandler[] mFuncArray = null;

        private Dictionary<string, int> mUserMap = new Dictionary<string, int>();
        private Dictionary<int, ChatRoom> mRooms = new Dictionary<int, ChatRoom>();

        public void StartService()
        {
            if (mFuncArray != null)
                return;

            mFuncArray = new ICDPacketMgr.PacketHandler[DEF.CMD_MAX_COUNT];
            mFuncArray[DEF.CMD_NewUser] = ICD_NewUser;
            mFuncArray[DEF.CMD_UserList] = ICD_UserList;
            mFuncArray[DEF.CMD_Login] = ICD_Login;
            mFuncArray[DEF.CMD_Logout] = ICD_Logout;
            mFuncArray[DEF.CMD_TaskNew] = ICD_NewTask;
            mFuncArray[DEF.CMD_TaskEdit] = ICD_EditTask;
            mFuncArray[DEF.CMD_TaskList] = ICD_TaskList;
            mFuncArray[DEF.CMD_NewChat] = ICD_NewChat;
            mFuncArray[DEF.CMD_ChatMsg] = ICD_ProcChat;
            mFuncArray[DEF.CMD_AddChatUser] = ICD_ProcChat;
            mFuncArray[DEF.CMD_DelChatUser] = ICD_ProcChat;
            mFuncArray[DEF.CMD_ShowChat] = ICD_ProcChat;
            mFuncArray[DEF.CMD_HideChat] = ICD_ProcChat;

            ICDPacketMgr.GetInst().StartServiceServer();

            ICDPacketMgr.GetInst().OnRecv += (id, obj) =>
            {
                mFuncArray[obj.msgID]?.Invoke(id, obj);
            };

            ICDPacketMgr.GetInst().OnDisConnected += (id, obj) =>
            {
                DelUser(id);
            };
        }

        private void ICD_NewUser(int clientID, HEADER obj)
        {
            User msg = obj as User;

            if (DatabaseMgr.GetUserInfo(msg.userID) != null)
            {
                HEADER pack = new HEADER();
                pack.FillServerHeader(DEF.CMD_NewUser);
                pack.msgErr = DEF.ERR_HaveID;
                ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
            }
            else
            {
                DatabaseMgr.NewUser(msg);

                HEADER pack = new HEADER();
                pack.FillServerHeader(DEF.CMD_NewUser);
                ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
            }
        }

        private void ICD_UserList(int clientID, HEADER obj)
        {
            DataTable table = DatabaseMgr.GetUsers();
            if (table == null)
                return;

            Message pack = new Message();
            pack.FillServerHeader(DEF.CMD_UserList);
            foreach(DataRow user in table.Rows)
            {
                pack.message += user["id"].ToString();
                pack.message += ",";
            }

            ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
        }

        private void ICD_Login(int clientID, HEADER obj)
        {
            User msg = obj as User;
            DataRow row = DatabaseMgr.GetUserInfo(msg.userID);
            User pack = new User();
            pack.FillServerHeader(DEF.CMD_Login);
            if (row != null)
            {
                if (row["pw"].ToString() == msg.userPW)
                {
                    AddUser(clientID, msg.userID);
                    pack.userID = row["id"].ToString();
                    pack.userPW = row["pw"].ToString();
                }
                else
                    pack.msgErr = DEF.ERR_WorngPW;
            }
            else
            {
                pack.msgErr = DEF.ERR_NoID;
            }

            ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
        }

        private void ICD_Logout(int clientID, HEADER obj)
        {
            DelUser(obj.msgUser);
            HEADER pack = new HEADER();
            pack.FillServerHeader(DEF.CMD_Logout);
            ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
        }

        private void ICD_NewTask(int clientID, HEADER obj)
        {
            ICD.Task msg = obj as ICD.Task;
            DataRow row = DatabaseMgr.NewTask(msg);
            int taskID = (int)row["recordID"];

            ICD.Task task = new ICD.Task();
            task.FillServerHeader(DEF.CMD_TaskInfo);
            DatabaseMgr.GetTaskLatest(taskID, ref task);
            sendMsg(task.worker, task);
            sendMsg(task.director, task);
        }
        private void ICD_EditTask(int clientID, HEADER obj)
        {
            TaskEdit msg = obj as TaskEdit;
            DatabaseMgr.EditTask(msg);
            int taskID = msg.taskID;

            ICD.Task task = new ICD.Task();
            task.FillServerHeader(DEF.CMD_TaskInfo);
            DatabaseMgr.GetTaskLatest(taskID, ref task);
            sendMsg(task.worker, task);
            sendMsg(task.director, task);
        }
        private void ICD_TaskList(int clientID, HEADER obj)
        {
            DataTable table = DatabaseMgr.GetTasks(obj.msgUser);
            if (table == null)
                return;

            foreach(DataRow row in table.Rows)
            {
                ICD.Task task = new ICD.Task();
                task.FillServerHeader(DEF.CMD_TaskInfo);
                DatabaseMgr.GetTaskLatest((int)row["recordID"], ref task);
                sendMsg(obj.msgUser, task);
            }
        }

        private void ICD_NewChat(int clientID, HEADER obj)
        {
            Chat msg = (Chat)obj;
            int chatID = DatabaseMgr.PushNewChat(msg);
            ChatRoom room = new ChatRoom();
            room.Init(chatID);
            mRooms[chatID] = room;
        }

        private void ICD_ProcChat(int clientID, HEADER obj)
        {
            Chat msg = (Chat)obj;
            if( !mRooms.ContainsKey(msg.recordID) )
            {
                LOG.warn();
                return;
            }

            mRooms[msg.recordID].ProcChat(msg);
        }

        public void sendMsg(string user, HEADER obj)
        {
            if(user==null || !mUserMap.ContainsKey(user))
                return;

            ICDPacketMgr.GetInst().sendMsgToClient(mUserMap[user], obj);
        }

        private void AddUser(int id, string UserName)
        {
            mUserMap[UserName] = id;
        }
        private void DelUser(string UserName)
        {
            if (!mUserMap.ContainsKey(UserName))
                return;

            mUserMap.Remove(UserName);
        }
        private void DelUser(int clientID)
        {
            foreach (var item in mUserMap)
            {
                if (item.Value == clientID)
                {
                    mUserMap.Remove(item.Key);
                    break;
                }
            }
        }
    }
}
