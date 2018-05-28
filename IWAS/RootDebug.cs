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
            
            //ICD.ChatRoomList msg1 = new ICD.ChatRoomList();
            //msg1.FillHeader(123);
            //msg1.body = new ICD.RoomInfo[3];
            //msg1.body[0] = new ICD.RoomInfo();
            //msg1.body[1] = new ICD.RoomInfo();
            //msg1.body[2] = new ICD.RoomInfo();
            //msg1.body[0].recordID = 1;
            //msg1.body[1].recordID = 2;
            //msg1.body[2].recordID = 3;
            //byte[] buf = msg1.Serialize();
            //ICD.ChatRoomList msg2 = new ICD.ChatRoomList();
            //msg2.Deserialize(ref buf);
            //ICD.HEADER head = msg2;
            
            Visible = false;
            Login form = new Login();
            form.ConnectServer();
            form.Show();

            //Dispose();
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
