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
            btnDirector.Text = MyInfo.mMyInfo.userID;
            btnWorker.Text = MyInfo.mMyInfo.userID;
        }

        private void btnNewTask_Click(object sender, EventArgs e)
        {
            if(cbType.Text == "채팅")
            {
                ICD.ChatRoomInfo msgNewChat = new ICD.ChatRoomInfo();
                msgNewChat.FillClientHeader(ICD.DEF.CMD_NewChat, 0);
                msgNewChat.body.access = cbAccess.Text;
                if(btnDirector.Text == btnWorker.Text)
                {
                    msgNewChat.body.users = new string[1];
                    msgNewChat.body.users[0] = btnDirector.Text;
                }
                else
                {
                    msgNewChat.body.users = new string[2];
                    msgNewChat.body.users[0] = btnDirector.Text;
                    msgNewChat.body.users[1] = btnWorker.Text;
                }
                ICDPacketMgr.GetInst().sendMsgToServer(msgNewChat);

            }
            else
            {
                ICD.WorkList msg = new ICD.WorkList();
                msg.FillClientHeader(ICD.DEF.CMD_TaskNew, 0);
                ICD.Work msgTask = msg.works[0];

                //comboBox listing
                msgTask.type = cbType.Text;
                msgTask.access = cbAccess.Text;
                msgTask.mainCate = cbMainCate.Text;
                msgTask.subCate = cbSubCate.Text;
                //formatting date
                msgTask.launch = dtLaunch.Value.ToString("yyyy-MM-dd HH:mm:ss");
                msgTask.due = dtDue.Value.ToString("yyyy-MM-dd HH:mm:ss");
                msgTask.term = dtTerm.Value.ToString("yyyy-MM-dd HH:mm:ss");
                msgTask.priority = cbPriority.Text;
                //user listing
                msgTask.creator = MyInfo.mMyInfo.userID;
                msgTask.director = btnDirector.Text;
                msgTask.worker = btnWorker.Text;
                msgTask.title = tbTitle.Text;
                msgTask.comment = tbComment.Text;
                msgTask.state = "예정";

                msgTask.timeFirst = msg.msgTime;
                msgTask.timeDone = DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss");

                ICDPacketMgr.GetInst().sendMsgToServer(msg);
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
