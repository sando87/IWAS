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
            ICD.Task msgTask = new ICD.Task();
            ICD.HEADER.FillHeader(msgTask, ICD.COMMAND.TaskNew, ICD.TYPE.REQ, "root");

            msgTask.kind = cbType.Text;
            msgTask.access = cbAccess.Text;
            msgTask.mainCategory = cbMainCate.Text;
            msgTask.subCategory = cbSubCate.Text;
            msgTask.preLaunch = tbLaunch.Text;
            msgTask.preDue = tbDue.Text;
            msgTask.preterm = tbTerm.Text;
            msgTask.priority = cbPriority.Text;
            msgTask.director = cbDirector.Text;
            msgTask.worker = cbWorker.Text;
            msgTask.title = tbTitle.Text;
            msgTask.comment = tbComment.Text;

            ICDPacketMgr.GetInst().sendMsgToServer(msgTask);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
