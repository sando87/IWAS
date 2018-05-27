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
    public partial class DlgChatRoom : Form
    {
        private int mRoomID = 0;
        Dictionary<int, ChatRoom.MsgInfo> mMsgList = new Dictionary<int, ChatRoom.MsgInfo>();

        public DlgChatRoom(int id)
        {
            InitializeComponent();
            mRoomID = id;
        }

        private void DlgChatRoom_Load(object sender, EventArgs e)
        {
            InitListViews();

            ICDPacketMgr.GetInst().OnRecv += OnProcChatRoom;
            FormClosed += delegate {
                ICDPacketMgr.GetInst().OnRecv -= OnProcChatRoom;

                Chat msgOut = new Chat();
                msgOut.FillClientHeader(DEF.CMD_HideChat);
                msgOut.recordID = mRoomID;
                ICDPacketMgr.GetInst().sendMsgToServer(msgOut);
            };

            Chat msgAll = new Chat();
            msgAll.FillClientHeader(DEF.CMD_ChatMsgAll);
            msgAll.recordID = mRoomID;
            ICDPacketMgr.GetInst().sendMsgToServer(msgAll);

            Chat msgIn = new Chat();
            msgIn.FillClientHeader(DEF.CMD_ShowChat);
            msgIn.recordID = mRoomID;
            ICDPacketMgr.GetInst().sendMsgToServer(msgIn);

        }

        private void InitListViews()
        {
            lvMsg.View = View.Details;
            lvMsg.GridLines = true;
            lvMsg.FullRowSelect = true;
            lvMsg.Sorting = SortOrder.None;
            lvMsg.Columns.Add("id");
            lvMsg.Columns[0].Width = 50;
            lvMsg.Columns.Add("user");
            lvMsg.Columns[1].Width = 50;
            lvMsg.Columns.Add("message");
            lvMsg.Columns[2].Width = 200;
            lvMsg.Columns.Add("state");
            lvMsg.Columns[3].Width = 50;
        }

        private void UpdateMsgListView()
        {
            foreach(var msg in mMsgList.Reverse())
            {
                var val = msg.Value;
                if (val.isSignaled == false)
                    break;

                if (val.index >= lvMsg.Items.Count)
                {
                    string[] infos = new string[4];
                    infos[0] = val.msgID.ToString();
                    infos[1] = val.user;
                    infos[2] = val.message;
                    infos[3] = val.tick.ToString();
                    ListViewItem pack = new ListViewItem(infos);
                    lvMsg.Items.Add(pack);
                }
                else
                {
                    var item = lvMsg.Items[val.index];
                    item.SubItems[0].Text = val.msgID.ToString();
                    item.SubItems[1].Text = val.user;
                    item.SubItems[2].Text = val.message;
                    item.SubItems[3].Text = val.tick.ToString();
                }
            }
        }

        private void OnProcChatRoom(int clientID, ICD.HEADER obj)
        {
            ICD.Chat msg = (ICD.Chat)obj;
            switch (msg.msgID)
            {
                case ICD.DEF.CMD_ChatMsgList:
                    ProcMsgList(msg);
                    break;
                case ICD.DEF.CMD_DelChatUser:
                    ProcDelUser(msg);
                    break;
                default:
                    break;
            }
        }


        private void ProcMsgList(Chat msg)
        {
            string[] chatMsgs = msg.info.Split('\0');
            foreach(string chat in chatMsgs.Reverse())
            {
                if (chat.Length == 0)
                    continue;

                string[] infos = chat.Split(',', (char)5);
                ChatRoom.MsgInfo item = new ChatRoom.MsgInfo();
                item.msgID = int.Parse(infos[0]);
                item.tick = int.Parse(infos[1]);
                item.time = infos[2];
                item.user = infos[3];
                item.message = infos[4];
                item.isSignaled = true;
                item.index = mMsgList.Count;

                mMsgList[item.msgID] = item;
            }
            UpdateMsgListView();
        }

        private void ProcDelUser(Chat msg)
        {
            mMsgList.Clear();
            lvMsg.Clear();
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tbMsg.Text.Length == 0)
                return;

            Chat msg = new Chat();
            msg.FillClientHeader(DEF.CMD_ChatMsg);
            msg.recordID = mRoomID;
            msg.info = tbMsg.Text;

            ICDPacketMgr.GetInst().sendMsgToServer(msg);
            tbMsg.Text = "";
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            DlgEditChatUsers dlg = new DlgEditChatUsers(mRoomID);
            dlg.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Chat msg = new Chat();
            msg.FillClientHeader(DEF.CMD_DelChatUser);
            msg.recordID = mRoomID;
            msg.info = MyInfo.mMyInfo.userID;
            ICDPacketMgr.GetInst().sendMsgToServer(msg);
            Close();
        }

        private void tbMsg_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Shift == false && e.KeyCode == Keys.Enter)
            {
                btnSend_Click(null, null);
            }
        }
    }
}
