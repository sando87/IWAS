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
    public partial class TaskWindow : Form
    {
        private ICD.Task mTask = new ICD.Task();
        private bool isEditable = false;

        public TaskWindow(ICD.Task task)
        {
            InitializeComponent();

//            mTask = task;
        }

        ~TaskWindow()
        {
            LOG.trace();
            ICDPacketMgr.GetInst().OnRecv -= OnRecvEditTask;
            // += -= 메커니즘 확인 필요~!!
        }

        private void TaskWindow_Load(object sender, EventArgs e)
        {
            mTask.preLaunch = "870406";
            mTask.title = "testsetset";
            mTask.comment = "123123";
            //mTask.kind = "플젝";

            updateTaskInfo();

            ICDPacketMgr.GetInst().OnRecv += OnRecvEditTask;
        }

        

        private void OnRecvEditTask(int clientID, ICD.HEADER obj)
        {
            if (ICD.COMMAND.TaskInfo == (ICD.COMMAND)obj.msgID)
            {
                ICD.Task msg = (ICD.Task)obj;
                mTask = msg;
                updateTaskInfo();
            }
        }


        private void updateTaskInfo()
        {
            btnWorker.Text = mTask.worker;
            btnDirector.Text = mTask.director;
            tbComment.Text = mTask.comment;
            tbTitle.Text = mTask.title;
            tbTerm.Text = mTask.preterm;
            tbDue.Text = mTask.preDue;
            tbLaunch.Text = mTask.preLaunch;
            cbPriority.Text = mTask.priority;
            cbSubCate.Text = mTask.subCategory;
            cbMainCate.Text = mTask.mainCategory;
            cbAccess.Text = mTask.access;
            cbType.Text = mTask.kind;
        }

        private void enableAllCtrls(bool isEn)
        {
            btnWorker.Enabled   = isEn;
            btnDirector.Enabled = isEn;
            tbComment.Enabled   = isEn;
            tbTitle.Enabled     = isEn;
            tbTerm.Enabled      = isEn;
            tbDue.Enabled       = isEn;
            tbLaunch.Enabled    = isEn;
            cbPriority.Enabled  = isEn;
            cbSubCate.Enabled   = isEn;
            cbMainCate.Enabled  = isEn;
            cbAccess.Enabled    = isEn;
            cbType.Enabled      = isEn;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            isEditable = !isEditable;

            enableAllCtrls(isEditable);

            btnReqFix.Visible = isEditable;
            btnEdit.Text = isEditable ? "취소" : "편집";
            if(!isEditable)
            {
                updateTaskInfo();
            }
        }

        private void sendEditInfo()
        {
            ICD.TaskEdit msg = new ICD.TaskEdit();
            msg.FillClientHeader(ICD.COMMAND.TaskEdit);
            msg.editTaskID = mTask.taskID;
            msg.info = "";

            if (tbTitle.Text != mTask.title)
                msg.info += string.Format("title:{0},", tbTitle.Text);

            if (tbComment.Text != mTask.comment)
                msg.info += string.Format("comment:{0},", tbComment.Text);

            if (tbLaunch.Text != mTask.preLaunch)
                msg.info += string.Format("launch:{0},", tbLaunch.Text);

            if (msg.info.Length == 0)
            {
                MessageBox.Show("변경점이 없습니다.");
            }
            else
            {
                ICDPacketMgr.GetInst().sendMsgToServer(msg);
            }
            
        }

        private void btnReqFix_Click(object sender, EventArgs e)
        {
            sendEditInfo();
            isEditable = false;
            btnReqFix.Visible = false;
            btnEdit.Text = "편집";
            updateTaskInfo();
            enableAllCtrls(false);
        }
    }
}
