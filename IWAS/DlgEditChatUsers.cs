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
        public string[] mChatUserList = null;
        public string[] mChatNewList = null;
        public DlgEditChatUsers(int id)
        {
            InitializeComponent();
            mRoomID = id;
        }

        private void DlgEditChatUsers_Load(object sender, EventArgs e)
        {
            InitListViews();

            InitUserAll();

            ICDPacketMgr.GetInst().OnRecv += OnProcUserList;
            FormClosed += delegate {
                ICDPacketMgr.GetInst().OnRecv -= OnProcUserList;
            };

            ChatRoomInfo msg = new ChatRoomInfo();
            msg.FillClientHeader(DEF.CMD_ChatRoomInfo, 0);
            msg.body.recordID = mRoomID;
            ICDPacketMgr.GetInst().sendMsgToServer(msg);
        }

        private void OnProcUserList(int id, HEADER obj)
        {
            ChatRoomInfo msg = (ChatRoomInfo)obj;

            if(msg.body.users == null || msg.body.users.Length==0)
            {
                LOG.warn();
                return;
            }

            int cnt = msg.body.users.Length;
            mChatUserList = new string[cnt];
            for (int i=0; i< cnt; ++i)
            {
                string name = msg.body.users[i];
                mChatUserList[i] = name;
                lvCurUsers.Items.Add(name);
            }
        }

        public string[] GetUsers()
        {
            return mChatUserList;
        }

        private void InitUserAll()
        {
            foreach (string item in UserList.mUserList)
                lvUserAll.Items.Add(item);
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
            int cnt = lvCurUsers.Items.Count;
            mChatNewList = new string[cnt];
            for (int i = 0; i < cnt; i++)
            {
                mChatNewList[i] = lvCurUsers.Items[i].SubItems[0].Text;
            }
            Close();
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
