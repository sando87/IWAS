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

            Dispose();
            //Relese All
            //Close databases
            //Close Network Socket
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            DatabaseMgr.Open();
            MsgCtrl.GetInst().StartService();
        }
    }
}
