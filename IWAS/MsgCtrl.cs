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
            mFuncArray[DEF.CMD_TaskIDList] = ICD_TaskList;

            mFuncArray[DEF.CMD_NewChat] = ICD_NewChat;
            mFuncArray[DEF.CMD_ChatMsg] = ICD_ProcChat;
            mFuncArray[DEF.CMD_AddChatUsers] = ICD_ProcChat;
            mFuncArray[DEF.CMD_DelChatUsers] = ICD_ProcChat;
            mFuncArray[DEF.CMD_ShowChat] = ICD_ProcChat;
            mFuncArray[DEF.CMD_HideChat] = ICD_ProcChat;
            mFuncArray[DEF.CMD_ChatMsgAll] = ICD_ProcChat;
            mFuncArray[DEF.CMD_ChatRoomInfo] = ICD_ProcChat;
            mFuncArray[DEF.CMD_ChatRoomList] = ICD_ChatRoomList;

            mFuncArray[DEF.CMD_TaskBaseList] = ICD_ProcWorkList;
            mFuncArray[DEF.CMD_TaskHistory] = ICD_ProcWorkHistory;

            InitChatRooms();

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
                pack.message += user["recordID"].ToString();
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
                if (row["password"].ToString() == msg.userPW)
                {
                    AddUser(clientID, msg.userID);
                    pack.userID = row["recordID"].ToString();
                    pack.userPW = row["password"].ToString();
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
            ICD.WorkList msg = obj as ICD.WorkList;
            if(msg.works == null && msg.works.Length!=1)
            {
                LOG.warn();
                return;
            }
            ICD.Work work = msg.works[0];

            ChatRoom room = new ChatRoom();
            ChatRoomInfo roomInfo = new ChatRoomInfo();
            roomInfo.FillHeader(obj);
            roomInfo.body.access = work.access;
            roomInfo.body.users = new string[2];
            roomInfo.body.users[0] = work.director;
            roomInfo.body.users[1] = work.worker;
            int chatID = room.CreateNewChat(roomInfo);
            mRooms[chatID] = room;
            roomInfo.body.recordID = chatID;

            work.chatID = chatID;
            DataRow row = DatabaseMgr.NewTask(msg);
            int taskID = (int)row["recordID"];
            roomInfo.body.taskIDs = new int[1];
            roomInfo.body.taskIDs[0] = taskID;
            room.AddTask(roomInfo);

            ICD.WorkList task = new ICD.WorkList();
            task.FillServerHeader(DEF.CMD_TaskLatestInfo, 0);
            DatabaseMgr.GetTaskLatest(taskID, ref task.works[0]);
            sendMsg(task.works[0].worker, task);
            sendMsg(task.works[0].director, task);
        }
        private void ICD_EditTask(int clientID, HEADER obj)
        {
            WorkHistoryList msg = obj as WorkHistoryList;
            DatabaseMgr.EditTask(msg);
            int taskID = msg.workHistory[0].taskID;

            if ((msg.workHistory.Length == 1) && (msg.workHistory[0].columnName == "confirmOK"))
            {
                string[] data = new string[2];
                data = msg.workHistory[0].toInfo.Split(',', (char)2);
                DatabaseMgr.EditTaskBase(taskID, "timeDone", data[0]);
            }

            ICD.WorkList task = new ICD.WorkList();
            task.FillServerHeader(DEF.CMD_TaskLatestInfo, 0);
            DatabaseMgr.GetTaskLatest(taskID, ref task.works[0]);
            sendMsg(task.works[0].worker, task);
            sendMsg(task.works[0].director, task);
        }
        private void ICD_TaskList(int clientID, HEADER obj)
        {
            DataTable table = DatabaseMgr.GetTasks(obj.msgUser);
            if (table == null || table.Rows.Count==0)
                return;

            ICD.WorkList msg = new ICD.WorkList(table.Rows.Count);
            msg.FillServerHeader(DEF.CMD_TaskIDList, 0);
            foreach (DataRow row in table.Rows)
            {
                int index = table.Rows.IndexOf(row);
                DatabaseMgr.GetTaskLatest((int)row["recordID"], ref msg.works[index]);
                int state = mRooms[msg.works[index].chatID].GetUserState(obj.msgUser);
                msg.works[index].currentState = state;
            }
            sendMsg(obj.msgUser, msg);
        }

        private void InitChatRooms()
        {
            DataTable table = DatabaseMgr.GetChatRoomList();
            if (table == null)
                return;

            foreach(DataRow row in table.Rows)
            {
                int chatID = (int)row["recordID"];
                ChatRoom room = new ChatRoom();
                room.Init(chatID);
                mRooms[chatID] = room;
            }
        }

        private void ICD_NewChat(int clientID, HEADER obj)
        {
            ChatRoom room = new ChatRoom();
            int chatID = room.CreateNewChat((ChatRoomInfo)obj);
            mRooms[chatID] = room;
        }

        private void ICD_ProcChat(int clientID, HEADER obj)
        {
            ChatRoomInfo msg = (ChatRoomInfo)obj;
            if( !mRooms.ContainsKey(msg.body.recordID) )
            {
                LOG.warn();
                return;
            }

            mRooms[msg.body.recordID].ProcChat(msg);
        }

        private void ICD_ProcWorkList(int clientID, HEADER obj)
        {
            string user = obj.msgUser;
            DataTable table = DatabaseMgr.GetTasks(obj.ext1, obj.ext2);
            if (table == null)
                return;

            WorkList msg = new WorkList();
            msg.FillServerHeader(DEF.CMD_TaskBaseList, 0);
            msg.works = new Work[table.Rows.Count];
            foreach (DataRow row in table.Rows)
            {
                int idx = table.Rows.IndexOf(row);
                msg.works[idx] = new Work();

                msg.works[idx].recordID = (int)row["recordID"];
                msg.works[idx].type = row["type"].ToString();
                msg.works[idx].time = row["time"].ToString();
                msg.works[idx].creator = row["creator"].ToString();
                msg.works[idx].access = row["access"].ToString();
                msg.works[idx].mainCate = row["mainCate"].ToString();
                msg.works[idx].subCate = row["subCate"].ToString();
                msg.works[idx].title = row["title"].ToString();
                msg.works[idx].comment = row["comment"].ToString();
                msg.works[idx].director = row["director"].ToString();
                msg.works[idx].worker = row["worker"].ToString();
                msg.works[idx].launch = row["launch"].ToString();
                msg.works[idx].due = row["due"].ToString();
                msg.works[idx].term = row["term"].ToString();
                msg.works[idx].state = row["state"].ToString();
                msg.works[idx].priority = row["priority"].ToString();
                msg.works[idx].progress = (int)row["progress"];
                msg.works[idx].chatID = (int)row["chatID"];
                msg.works[idx].timeFirst = row["timeFirst"].ToString();
                msg.works[idx].timeDone = row["timeDone"].ToString();
            }
            sendMsg(user, msg);
        }

        private void ICD_ProcWorkHistory(int clientID, HEADER obj)
        {
            string user = obj.msgUser;
            int taskID = int.Parse(obj.ext1);
            DataTable table = DatabaseMgr.GetTaskHistory(taskID);
            if (table == null)
                return;

            List<WorkHistory> vec = new List<WorkHistory>();
            foreach (DataRow item in table.Rows)
            {
                WorkHistory his = new WorkHistory();
                his.recordID = (int)item["recordID"];
                his.taskID = (int)item["taskID"];
                his.time = item["time"].ToString(); ;
                his.editor = item["user"].ToString();
                his.columnName = item["columnName"].ToString();
                his.fromInfo = item["fromInfo"].ToString();
                his.toInfo = item["toInfo"].ToString();

                vec.Add(his);
            }

            WorkHistoryList msg = new WorkHistoryList();
            msg.FillServerHeader(DEF.CMD_TaskHistory, 0);
            msg.workHistory = vec.ToArray();
            sendMsg(user, msg);
        }

        private void ICD_ChatRoomList(int clientID, HEADER obj)
        {
            List<RoomInfo> vec = new List<RoomInfo>();
            foreach (var item in mRooms)
            {
                if (!item.Value.IsUser(obj.msgUser))
                    continue;

                RoomInfo info = new RoomInfo();
                ChatRoomInfo room = item.Value.GetRoomInfo();
                info.recordID = room.body.recordID;
                info.state = item.Value.GetUserState(obj.msgUser);
                info.users = room.body.users;
                vec.Add(info);
            }

            if(vec.Count > 0)
            {
                ChatRoomList msg = new ChatRoomList(1);
                msg.FillServerHeader(DEF.CMD_ChatRoomList, 0);
                msg.body = vec.ToArray();
                sendMsg(obj.msgUser, msg);
            }
            
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
