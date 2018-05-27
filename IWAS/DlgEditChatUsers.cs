using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IWAS.ICD;

namespace IWAS
{
    public partial class DlgEditChatUsers : Form
    {
        private int mRoomID = 0;
        public DlgEditChatUsers(int id)
        {
            InitializeComponent();
            mRoomID = id;
        }

        private void DlgEditChatUsers_Load(object sender, EventArgs e)
        {
            InitListViews();

            ICDPacketMgr.GetInst().OnRecv += OnProcChatUsers;
            FormClosed += delegate {
                ICDPacketMgr.GetInst().OnRecv -= OnProcChatUsers;
            };

            Chat msgUser = new Chat();
            msgUser.FillClientHeader(DEF.CMD_ChatRoomInfo);
            msgUser.recordID = mRoomID;
            ICDPacketMgr.GetInst().sendMsgToServer(msgUser);

            ICD.HEADER msgAll = new ICD.HEADER();
            msgAll.FillClientHeader(ICD.DEF.CMD_UserList);
            ICDPacketMgr.GetInst().sendMsgToServer(msgAll);
        }

        private void InitListViews()
        {
            lvUserAll.View = View.Details;
            lvUserAll.GridLines = true;
            lvUserAll.FullRowSelect = true;
            lvUserAll.Sorting = SortOrder.Ascending;
            lvUserAll.Columns.Add("name");
            lvUserAll.Columns[0].Width = 300;

            lvCurUsers.View = View.Details;
            lvCurUsers.GridLines = true;
            lvCurUsers.FullRowSelect = true;
            lvCurUsers.Sorting = SortOrder.Ascending;
            lvCurUsers.Columns.Add("name");
            lvCurUsers.Columns[0].Width = 300;

        }

        private void OnProcChatUsers(int clientID, ICD.HEADER obj)
        {
            if (obj.msgID == DEF.CMD_ChatRoomInfo)
            {
                Chat msg = (Chat)obj;
                string[] infos = msg.info.Split(',');
                foreach (string info in infos)
                {
                    if (info.Length > 0)
                    {
                        lvCurUsers.Items.Add(info);
                    }
                }
            }
            else if (obj.msgID == DEF.CMD_UserList)
            {
                ICD.Message msg = (ICD.Message)obj;
                string[] infos = msg.message.Split(',');
                foreach (string info in infos)
                {
                    if (info.Length > 0)
                    {
                        lvUserAll.Items.Add(info);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var indices = lvUserAll.SelectedIndices;
            for(int i= indices.Count-1; i>=0; --i)
            {
                int itemIdx = indices[i];
                string user = lvUserAll.Items[itemIdx].SubItems[0].Text;
                AddUser(user);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var indices = lvCurUsers.SelectedIndices;
            for (int i = indices.Count - 1; i >= 0; --i)
            {
                int itemIdx = indices[i];
                lvCurUsers.Items.RemoveAt(itemIdx);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string users = "";
            int cnt = lvCurUsers.Items.Count;
            for (int i = 0; i < cnt; i++)
            {
                users += lvCurUsers.Items[i].SubItems[0].Text;
                users += ",";
            }

            Chat msg = new Chat();
            msg.FillClientHeader(DEF.CMD_SetChatUsers);
            msg.recordID = mRoomID;
            msg.info = users;
            ICDPacketMgr.GetInst().sendMsgToServer(msg);
        }

        private void AddUser(string user)
        {
            int cnt = lvCurUsers.Items.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (user == lvCurUsers.Items[i].SubItems[0].Text)
                    return;
            }

            lvCurUsers.Items.Add(user);
        }

        private void lvUserAll_DoubleClick(object sender, EventArgs e)
        {
            if (lvUserAll.SelectedItems.Count == 1)
            {
                var items = lvUserAll.SelectedItems;
                ListViewItem lvItem = items[0];
                string user = lvItem.SubItems[0].Text;
                AddUser(user);
            }
        }

        private void lvCurUsers_DoubleClick(object sender, EventArgs e)
        {
            if (lvCurUsers.SelectedItems.Count == 1)
            {
                var items = lvCurUsers.SelectedItems;
                lvCurUsers.Items.Remove(items[0]);
            }
        }
    }
}
