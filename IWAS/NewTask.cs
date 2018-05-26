using System;
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
    public partial class NewTask : Form
    {
        public NewTask()
        {
            InitializeComponent();
        }

        private void btnNewTask_Click(object sender, EventArgs e)
        {
            if(cbType.Text == "채팅")
            {
                ICD.Chat msgNewChat = new ICD.Chat();
                msgNewChat.FillClientHeader(ICD.DEF.CMD_NewChat);
                msgNewChat.access = cbAccess.Text;
                if(btnDirector.Text == btnWorker.Text)
                {
                    msgNewChat.info = btnDirector.Text;
                }
                else
                {
                    msgNewChat.info = btnDirector.Text + "," + btnWorker.Text;
                }
                ICDPacketMgr.GetInst().sendMsgToServer(msgNewChat);

            }
            else
            {
                ICD.Task msgTask = new ICD.Task();
                msgTask.FillClientHeader(ICD.DEF.CMD_TaskNew);

                //comboBox listing
                msgTask.kind = cbType.Text;
                msgTask.access = cbAccess.Text;
                msgTask.mainCategory = cbMainCate.Text;
                msgTask.subCategory = cbSubCate.Text;
                //formatting date
                msgTask.preLaunch = tbLaunch.Text;
                msgTask.preDue = tbDue.Text;
                msgTask.preterm = tbTerm.Text;
                msgTask.priority = cbPriority.Text;
                //user listing
                msgTask.creator = MyInfo.mMyInfo.userID;
                msgTask.director = btnDirector.Text;
                msgTask.worker = btnWorker.Text;
                msgTask.title = tbTitle.Text;
                msgTask.comment = tbComment.Text;

                ICDPacketMgr.GetInst().sendMsgToServer(msgTask);
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDirector_Click(object sender, EventArgs e)
        {
            UserList dlg = new UserList();
            dlg.ShowDialog();
            if(dlg.mSelectedUser != null)
            {
                btnDirector.Text = dlg.mSelectedUser;
            }
        }

        private void btnWorker_Click(object sender, EventArgs e)
        {
            UserList dlg = new UserList();
            dlg.ShowDialog();
            if (dlg.mSelectedUser != null)
            {
                btnWorker.Text = dlg.mSelectedUser;
            }
        }
    }
}
