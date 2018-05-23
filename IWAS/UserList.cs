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
    public partial class UserList : Form
    {
        static public List<string> mUserList = new List<string>();
        public string mSelectedUser = null;
        public UserList()
        {
            InitializeComponent();
            InitListView();
            ICDPacketMgr.GetInst().OnRecv += OnRecvUserList;
        }

        private void OnRecvUserList(int clientID, ICD.HEADER obj)
        {
            if (ICD.COMMAND.UserList == (ICD.COMMAND)obj.msgID)
            {
                ICD.Message msg = (ICD.Message)obj;
                string[] infos = msg.message.Split(',');

                lvUserlist.Items.Clear();
                mUserList.Clear();
                foreach (string info in infos)
                {
                    if(info.Length>0)
                    {
                        mUserList.Add(info);
                        lvUserlist.Items.Add(info);
                    }
                }
            }
        }

        private void InitListView()
        {
            lvUserlist.View = View.Details;
            lvUserlist.GridLines = true;
            lvUserlist.FullRowSelect = true;
            lvUserlist.Sorting = SortOrder.Ascending;

            lvUserlist.Columns.Add("name");
            lvUserlist.Columns[0].Width = 300;

            foreach(var user in mUserList)
            {
                lvUserlist.Items.Add(user);
            }
        }
        private void lvUserlist_DoubleClick(object sender, EventArgs e)
        {
            if (lvUserlist.SelectedItems.Count == 1)
            {
                mSelectedUser = lvUserlist.SelectedItems[0].Text;
                ICDPacketMgr.GetInst().OnRecv -= OnRecvUserList;
                Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ICD.HEADER msg = new ICD.HEADER();
            msg.FillClientHeader(ICD.COMMAND.UserList);
            ICDPacketMgr.GetInst().sendMsgToServer(msg);
        }
    }
}
