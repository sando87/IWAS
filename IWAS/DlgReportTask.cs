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
        public bool mIsReport = false;
        public string Type { get; set; }
        public string Date { get; set; }
        public string Msg { get; set; }

        public DlgReportTask()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            mIsReport = true;
            Type = cbType.Text;
            Date = tbDate.Text;
            Msg = tbComment.Text;
        }
    }
}
