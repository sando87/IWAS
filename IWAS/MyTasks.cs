﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IWAS
{
    public partial class MyTasks : Form
    {
        Dictionary<uint, ICD.Task> mTasks;

        public MyTasks()
        {
            InitializeComponent();
            mTasks = new Dictionary<uint, ICD.Task>();
            ICDPacketMgr.GetInst().OnRecv += OnRecv_ICDMessages;
            InitListView();
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
            TaskList.Clear();

            foreach (KeyValuePair<uint, ICD.Task> task in mTasks)
            {
                AddListView(task.Value);
            }
        }
        private void AddListView(ICD.Task task)
        {
            string [] infos = new string[2];
            infos[0] = task.taskID.ToString();
            infos[1] = task.title;

            ListViewItem lvItems = new ListViewItem(infos);
            TaskList.Items.Add(lvItems);
        }

        private void OnRecv_ICDMessages(int clientID, ICD.HEADER o)
        {
            ICD.HEADER obj = o as ICD.HEADER;
            switch ((ICD.COMMAND)obj.id)
            {
                case ICD.COMMAND.TaskInfo:
                    OnRecv_TaskInfo(obj);
                    break;
                default:
                    break;
            }
        }

        private void OnRecv_TaskInfo(ICD.HEADER obj)
        {
            if(ICD.ERRORCODE.NOERROR != (ICD.ERRORCODE)obj.error)
            {
                LOG.warn();
                return;
            }

            ICD.Task task = obj as ICD.Task;
            mTasks[task.taskID] = task;
            AddListView(task);
        }

        private void NewTask_Click(object sender, EventArgs e)
        {
            NewTask newTask = new NewTask();
            newTask.ShowDialog();
            ICD.Task msg = newTask.mNewTaskInfo;
            ICDPacketMgr.GetInst().sendMsgToServer(msg);
        }

        private void TaskList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(TaskList.SelectedItems.Count == 1)
            {
                var items = TaskList.SelectedItems;
                ListViewItem lvItem = items[0];
                string strID = lvItem.SubItems[0].Text;
                uint taskID = uint.Parse(strID);
                ICD.Task objTask = mTasks[taskID];
                TaskWindow window = new TaskWindow(objTask);
                window.ShowDialog();
            }
        }
    }
}