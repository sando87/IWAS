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
        string[] mUsers = null;
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

                ChatRoomInfo msgOut = new ChatRoomInfo();
                msgOut.FillClientHeader(DEF.CMD_HideChat, 0);
                msgOut.body.recordID = mRoomID;
                ICDPacketMgr.GetInst().sendMsgToServer(msgOut);
            };

            ChatRoomInfo msgAll = new ChatRoomInfo();
            msgAll.FillClientHeader(DEF.CMD_ChatMsgAll, 0);
            msgAll.body.recordID = mRoomID;
            ICDPacketMgr.GetInst().sendMsgToServer(msgAll);

            ChatRoomInfo msgIn = new ChatRoomInfo();
            msgIn.FillClientHeader(DEF.CMD_ShowChat, 0);
            msgIn.body.recordID = mRoomID;
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
            lvMsg.Columns[1].Width = 100;
            lvMsg.Columns.Add("message");
            lvMsg.Columns[2].Width = 100;
            lvMsg.Columns.Add("state");
            lvMsg.Columns[3].Width = 50;
        }

        private void UpdateMsgListView(int id)
        {
            var msg = mMsgList[id];
            if (msg.index >= lvMsg.Items.Count)
            {
                string[] infos = new string[4];
                infos[0] = msg.msgID.ToString();
                infos[1] = msg.user;
                infos[2] = msg.message;
                infos[3] = msg.tick.ToString();
                ListViewItem pack = new ListViewItem(infos);
                lvMsg.Items.Add(pack);
            }
            else
            {
                var item = lvMsg.Items[msg.index];
                item.SubItems[0].Text = msg.msgID.ToString();
                item.SubItems[1].Text = msg.user;
                item.SubItems[2].Text = msg.message;
                item.SubItems[3].Text = msg.tick.ToString();
            }
        }

        private void OnProcChatRoom(int clientID, ICD.HEADER obj)
        {
            ICD.ChatRoomInfo msg = (ICD.ChatRoomInfo)obj;
            switch (msg.msgID)
            {
                case ICD.DEF.CMD_ChatRoomInfo:
                    ProcMsgList(msg);
                    break;
                case ICD.DEF.CMD_DelChatUsers:
                    ProcDelUser(msg);
                    break;
                default:
                    break;
            }
        }


        private void ProcMsgList(ChatRoomInfo msg)
        {
            mUsers = msg.body.users;

            if (msg.body.mesgs == null)
                return;

            foreach(var chat in msg.body.mesgs.Reverse())
            {
                int msgID = chat.recordID;
                if (mMsgList.ContainsKey(msgID))
                {
                    mMsgList[msgID].tick = chat.tick;
                    mMsgList[msgID].isSignaled = true;
                }
                else
                {
                    ChatRoom.MsgInfo item = new ChatRoom.MsgInfo();
                    item.msgID = msgID;
                    item.tick = chat.tick;
                    item.time = chat.time;
                    item.user = chat.user;
                    item.message = chat.message;
                    item.isSignaled = true;
                    item.index = mMsgList.Count;
                    mMsgList[msgID] = item;
                }
                UpdateMsgListView(msgID);
            }
            
        }

        private void ProcDelUser(ChatRoomInfo msg)
        {
            mMsgList.Clear();
            lvMsg.Clear();
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tbMsg.Text.Length == 0)
                return;

            ChatRoomInfo msg = new ChatRoomInfo();
            msg.FillClientHeader(DEF.CMD_ChatMsg, 0);
            msg.body.recordID = mRoomID;
            msg.body.mesgs = new MesgInfo[1];
            msg.body.mesgs[0] = new MesgInfo();
            msg.body.mesgs[0].message = tbMsg.Text;

            ICDPacketMgr.GetInst().sendMsgToServer(msg);
            tbMsg.Text = "";
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            DlgEditChatUsers dlg = new DlgEditChatUsers(mRoomID, mUsers);
            dlg.ShowDialog();

            if (dlg.mChatNewList == null)
                return;

            string[] pre = mUsers;
            string[] after = dlg.mChatNewList;

            {
                ChatRoomInfo msg = new ChatRoomInfo();
                msg.FillClientHeader(DEF.CMD_DelChatUsers, 0);
                msg.body.recordID = mRoomID;
                List<string> oldUsers = new List<string>();
                foreach (string item in pre)
                {
                    if (!after.Contains(item))
                    {
                        oldUsers.Add(item);
                    }
                }
                msg.body.users = oldUsers.ToArray();
                ICDPacketMgr.GetInst().sendMsgToServer(msg);
            }

            {
                ChatRoomInfo msg = new ChatRoomInfo();
                msg.FillClientHeader(DEF.CMD_AddChatUsers, 0);
                msg.body.recordID = mRoomID;
                List<string> newUsers = new List<string>();
                foreach(string item in after)
                {
                    if(!pre.Contains(item))
                    {
                        newUsers.Add(item);
                    }
                }
                msg.body.users = newUsers.ToArray();
                ICDPacketMgr.GetInst().sendMsgToServer(msg);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            ChatRoomInfo msg = new ChatRoomInfo();
            msg.FillClientHeader(DEF.CMD_DelChatUsers, 0);
            msg.body.recordID = mRoomID;
            msg.body.users = new string[1];
            msg.body.users[0] = MyInfo.mMyInfo.userID;
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
