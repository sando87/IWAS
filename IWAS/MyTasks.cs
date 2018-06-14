using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IWAS
{
    public partial class MyTasks : Form
    {
        static public Dictionary<int, ICD.RoomInfo> mChats = new Dictionary<int, ICD.RoomInfo>();
        static public Dictionary<int, ICD.Work> mTasks = new Dictionary<int, ICD.Work>();

        public MyTasks()
        {
            InitializeComponent();
        }

        private void MyTasks_Load(object sender, EventArgs e)
        {
            ICDPacketMgr.GetInst().OnRecv += OnRecv_ICDMessages;
            FormClosed += delegate{
                ICDPacketMgr.GetInst().OnRecv -= OnRecv_ICDMessages;
                Dispose();
                //Relese All
                //Close databases
                //Close Network Socket
                Process.GetCurrentProcess().Kill();
            };
            

            InitListView();

            ICD.HEADER msg = new ICD.HEADER();
            msg.FillClientHeader(ICD.DEF.CMD_TaskIDList);
            ICDPacketMgr.GetInst().sendMsgToServer(msg);

            /*
            ICD.HEADER chatMsg = new ICD.HEADER();
            chatMsg.FillClientHeader(ICD.DEF.CMD_ChatRoomList);
            ICDPacketMgr.GetInst().sendMsgToServer(chatMsg);
            */
        }

        private void InitListView()
        {
            TaskList.View = View.Details;
            TaskList.GridLines = true;
            TaskList.FullRowSelect = true;
            TaskList.Sorting = SortOrder.Ascending;
            TaskList.Columns.Add("id");
            TaskList.Columns[0].Width = 50;
            TaskList.Columns.Add("title");
            TaskList.Columns[1].Width = 100;
            TaskList.Columns.Add("state");
            TaskList.Columns[2].Width = 100;

            lvChat.View = View.Details;
            lvChat.GridLines = true;
            lvChat.FullRowSelect = true;
            lvChat.Sorting = SortOrder.Ascending;
            lvChat.Columns.Add("id");
            lvChat.Columns[0].Width = 50;
            lvChat.Columns.Add("title");
            lvChat.Columns[1].Width = 100;
            lvChat.Columns.Add("state");
            lvChat.Columns[2].Width = 100;

        }
        private void UpdateTaskList()
        {
            TaskList.Items.Clear();

            foreach (KeyValuePair<int, ICD.Work> task in mTasks)
            {
                AddListView(task.Value);
            }
        }
        private void UpdateChatList()
        {
            lvChat.Items.Clear();

            foreach (var chat in mChats)
            {
                AddListView(chat.Value);
            }
        }
        private void AddListView(ICD.Work task)
        {
            string [] infos = new string[3];
            infos[0] = task.recordID.ToString();
            infos[1] = task.title;
            infos[2] = task.currentState.ToString();

            ListViewItem lvItems = new ListViewItem(infos);
            TaskList.Items.Add(lvItems);
        }
        private void AddListView(ICD.RoomInfo chat)
        {
            string[] infos = new string[3];
            infos[0] = chat.recordID.ToString();
            infos[1] = chat.ToStringUserList();
            infos[2] = chat.state.ToString();

            ListViewItem lvItems = new ListViewItem(infos);
            lvChat.Items.Add(lvItems);
        }


        private void OnRecv_ICDMessages(int clientID, ICD.HEADER o)
        {
            ICD.HEADER obj = o as ICD.HEADER;
            switch (obj.msgID)
            {
                case ICD.DEF.CMD_TaskIDList:
                case ICD.DEF.CMD_TaskLatestInfo:
                    OnRecv_TaskInfo(obj);
                    break;
                case ICD.DEF.CMD_ChatRoomList:
                    OnRecv_ChatList(obj);
                    break;
                case ICD.DEF.CMD_ChatRoomInfo:
                    OnRecv_ChatRoomInfo(obj);
                    break;
                case ICD.DEF.CMD_DelChatUsers:
                    OnRecv_DelChatUser(obj);
                    break;
                default:
                    break;
            }
        }

        private void OnRecv_DelChatUser(ICD.HEADER obj)
        {
            ICD.ChatRoomInfo msg = (ICD.ChatRoomInfo)obj;
            if (mChats.ContainsKey(msg.body.recordID))
            {
                mChats.Remove(msg.body.recordID);
                UpdateChatList();
            }
        }

        private void OnRecv_ChatRoomInfo(ICD.HEADER obj)
        {
            ICD.ChatRoomInfo msg = (ICD.ChatRoomInfo)obj;
            foreach(int id in msg.body.taskIDs)
            {
                if (!mTasks.ContainsKey(id))
                    continue;

                mTasks[id].currentState = msg.body.state;
            }
            UpdateTaskList();

            //ICD.ChatRoomInfo msg = (ICD.ChatRoomInfo)obj;
            //mChats[msg.body.recordID] = msg.body;
            //UpdateChatList();
        }

        private void OnRecv_ChatList(ICD.HEADER obj)
        {
            ICD.ChatRoomList msg = (ICD.ChatRoomList)obj;

            foreach(ICD.RoomInfo item in msg.body)
                mChats[item.recordID] = item;

            UpdateChatList();
        }

        private void OnRecv_TaskInfo(ICD.HEADER obj)
        {
            if(ICD.DEF.ERR_NoError != obj.msgErr)
            {
                LOG.warn();
                return;
            }

            ICD.WorkList task = (ICD.WorkList)obj;
            foreach(var item in task.works)
                mTasks[item.recordID] = item;
            
            UpdateTaskList();
        }

        private void NewTask_Click(object sender, EventArgs e)
        {
            NewTask newTask = new NewTask();
            newTask.ShowDialog();
        }

        private void TaskList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(TaskList.SelectedItems.Count == 1)
            {
                var items = TaskList.SelectedItems;
                ListViewItem lvItem = items[0];
                string strID = lvItem.SubItems[0].Text;
                int taskID = int.Parse(strID);
                ICD.Work objTask = mTasks[taskID];
                TaskWindow window = new TaskWindow(objTask);
                window.ShowDialog();
            }
        }

        private void lvChat_DoubleClick(object sender, EventArgs e)
        {
            if (lvChat.SelectedItems.Count == 1)
            {
                var items = lvChat.SelectedItems;
                ListViewItem lvItem = items[0];
                string strID = lvItem.SubItems[0].Text;
                int chatID = int.Parse(strID);
                mChats[chatID].state = 0;
                lvItem.SubItems[2].Text = "0";
                DlgChatRoom room = new DlgChatRoom(chatID);
                room.ShowDialog();
            }

        }
    }
}
