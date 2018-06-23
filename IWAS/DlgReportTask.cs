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
    public partial class DlgReportTask : Form
    {
        public bool mIsOK = false;
        public string Type { get; set; }
        public long Time { get; set; }
        public string Msg { get; set; }

        public DlgReportTask(bool isReportType)
        {
            InitializeComponent();
            if(isReportType)
            {
                cbType.Items.Add("중간보고");
                cbType.Items.Add("최종보고");
                cbType.SelectedItem = 0;
                dtReport.Enabled = true;
                btnReport.Text = "Report";
            }
            else
            {
                cbType.Items.Add("승인");
                cbType.Items.Add("미승인");
                cbType.SelectedItem = 0;
                dtReport.Enabled = false;
                btnReport.Text = "Confirm";
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            mIsOK = true;
            Type = cbType.Text;
            Time = dtReport.Value.Ticks;
            Msg = tbComment.Text;
        }
    }
}
