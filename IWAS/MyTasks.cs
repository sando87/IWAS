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
        static public Dictionary<int, ICD.Task> mTasks = new Dictionary<int, ICD.Task>();

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
            msg.FillClientHeader(ICD.DEF.CMD_TaskList);
            ICDPacketMgr.GetInst().sendMsgToServer(msg);

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
            TaskList.Columns[1].Width = 300;

        }
        private void UpdateListView()
        {
            TaskList.Items.Clear();

            foreach (KeyValuePair<int, ICD.Task> task in mTasks)
            {
                AddListView(task.Value);
            }
        }
        private void AddListView(ICD.Task task)
        {
            string [] infos = new string[2];
            infos[0] = task.recordID.ToString();
            infos[1] = task.title;

            ListViewItem lvItems = new ListViewItem(infos);
            TaskList.Items.Add(lvItems);
        }

        private void OnRecv_ICDMessages(int clientID, ICD.HEADER o)
        {
            ICD.HEADER obj = o as ICD.HEADER;
            switch (obj.msgID)
            {
                case ICD.DEF.CMD_TaskInfo:
                    OnRecv_TaskInfo(obj);
                    break;
                default:
                    break;
            }
        }

        private void OnRecv_TaskInfo(ICD.HEADER obj)
        {
            if(ICD.DEF.ERR_NoError != obj.msgErr)
            {
                LOG.warn();
                return;
            }

            ICD.Task task = obj as ICD.Task;
            mTasks[task.recordID] = task;
            UpdateListView();
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
                ICD.Task objTask = mTasks[taskID];
                TaskWindow window = new TaskWindow(objTask);
                window.ShowDialog();
            }
        }
    }
}
