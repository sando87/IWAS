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

        public void StartService()
        {
            if (mFuncArray != null)
                return;

            mFuncArray = new ICDPacketMgr.PacketHandler[(uint)COMMAND.MAX_COUNT];
            mFuncArray[(uint)COMMAND.NewUser] = ICD_NewUser;
            mFuncArray[(uint)COMMAND.Login] = ICD_Login;
            mFuncArray[(uint)COMMAND.Logout] = ICD_Logout;
            mFuncArray[(uint)COMMAND.TaskNew] = ICD_NewTask;
            mFuncArray[(uint)COMMAND.TaskEdit] = ICD_EditTask;
            mFuncArray[(uint)COMMAND.TaskList] = ICD_TaskList;

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
                pack.FillServerHeader(COMMAND.NewUser);
                pack.msgErr = (uint)ERRORCODE.HaveID;
                ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
            }
            else
            {
                DatabaseMgr.NewUser(msg);

                HEADER pack = new HEADER();
                pack.FillServerHeader(COMMAND.NewUser);
                ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
            }
        }

        private void ICD_Login(int clientID, HEADER obj)
        {
            User msg = obj as User;
            DataRow row = DatabaseMgr.GetUserInfo(msg.userID);
            HEADER pack = new HEADER();
            pack.FillServerHeader(COMMAND.Login);
            if (row != null)
            {
                if(row["pw"].ToString() == msg.userPW)
                    AddUser(clientID, msg.userID);
                else
                    pack.msgErr = (uint)ERRORCODE.WorngPW;
            }
            else
            {
                pack.msgErr = (uint)ERRORCODE.NoID;
            }

            ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
        }

        private void ICD_Logout(int clientID, HEADER obj)
        {
            DelUser(obj.msgUser);
            HEADER pack = new HEADER();
            pack.FillServerHeader(COMMAND.Logout);
            ICDPacketMgr.GetInst().sendMsgToClient(clientID, pack);
        }

        private void ICD_NewTask(int clientID, HEADER obj)
        {
            ICD.Task msg = obj as ICD.Task;
            DataRow row = DatabaseMgr.NewTask(msg);
            int taskID = (int)row["recordID"];

            ICD.Task task = new ICD.Task();
            task.FillServerHeader(COMMAND.TaskInfo);
            DatabaseMgr.GetTaskLatest(taskID, ref task);
            sendMsg(task.worker, task);
            sendMsg(task.director, task);
        }
        private void ICD_EditTask(int clientID, HEADER obj)
        {
            TaskEdit msg = obj as TaskEdit;
            DatabaseMgr.EditTask(msg);
            int taskID = (int)msg.taskID;

            ICD.Task task = new ICD.Task();
            task.FillServerHeader(COMMAND.TaskInfo);
            DatabaseMgr.GetTaskLatest(taskID, ref task);
            sendMsg(task.worker, task);
            sendMsg(task.director, task);
        }
        private void ICD_TaskList(int clientID, HEADER obj)
        {
            DataTable table = DatabaseMgr.GetTasks(obj.msgUser);
            foreach(DataRow row in table.Rows)
            {
                ICD.Task task = new ICD.Task();
                task.FillServerHeader(COMMAND.TaskInfo);
                DatabaseMgr.GetTaskLatest((int)row["recordID"], ref task);
                sendMsg(obj.msgUser, task);
            }
        }


        public void sendMsg(string user, HEADER obj)
        {
            if(user==null || !mUserMap.ContainsKey(user))
            {
                LOG.warn();
                return;
            }

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
