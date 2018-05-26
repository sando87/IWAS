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
            ICDPacketMgr.GetInst().OnRecv += OnProcChatRoom;
            FormClosed += delegate {
                ICDPacketMgr.GetInst().OnRecv -= OnProcChatRoom;

                Chat msgOut = new Chat();
                msgOut.FillClientHeader(DEF.CMD_HideChat);
                msgOut.recordID = mRoomID;
                ICDPacketMgr.GetInst().sendMsgToServer(msgOut);
            };

            Chat msgIn = new Chat();
            msgIn.FillClientHeader(DEF.CMD_ShowChat);
            msgIn.recordID = mRoomID;
            ICDPacketMgr.GetInst().sendMsgToServer(msgIn);

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

                mMsgList[item.msgID] = item;
            }
        }

        private void ProcDelUser(Chat msg)
        {
            mMsgList.Clear();
            lvMsg.Clear();
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
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
