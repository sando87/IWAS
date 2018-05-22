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
    public partial class RootDebug : Form
    {
        public RootDebug()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Visible = false;
            Login form = new Login();
            form.ConnectServer();
            form.ShowDialog();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            MsgCtrl.GetInst().StartService();
        }
    }
}
