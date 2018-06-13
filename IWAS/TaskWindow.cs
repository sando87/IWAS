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
    public partial class TaskWindow : Form
    {
        private ICD.Work mTask = new ICD.Work();
        private bool isEditable = false;
        Dictionary<int, ChatRoom.MsgInfo> mMsgList = new Dictionary<int, ChatRoom.MsgInfo>();

        public TaskWindow(ICD.Work task)
        {
            InitializeComponent();

            mTask = task;
        }

        private void TaskWindow_Load(object sender, EventArgs e)
        {
            InitListViews();
            updateTaskInfo();

            ICDPacketMgr.GetInst().OnRecv += OnRecvEditTask;
            FormClosed += delegate{
                ICDPacketMgr.GetInst().OnRecv -= OnRecvEditTask;

                ChatRoomInfo msgOut = new ChatRoomInfo();
                msgOut.FillClientHeader(DEF.CMD_HideChat, 0);
                msgOut.body.recordID = mTask.chatID;
                ICDPacketMgr.GetInst().sendMsgToServer(msgOut);
            };

            ChatRoomInfo msgAll = new ChatRoomInfo();
            msgAll.FillClientHeader(DEF.CMD_ChatMsgAll, 0);
            msgAll.body.recordID = mTask.chatID;
            ICDPacketMgr.GetInst().sendMsgToServer(msgAll);

            ChatRoomInfo msgIn = new ChatRoomInfo();
            msgIn.FillClientHeader(DEF.CMD_ShowChat, 0);
            msgIn.body.recordID = mTask.chatID;
            ICDPacketMgr.GetInst().sendMsgToServer(msgIn);
        }

        

        private void OnRecvEditTask(int clientID, ICD.HEADER obj)
        {
            if (DEF.CMD_TaskInfo == obj.msgID)
            {
                ICD.WorkList msg = (ICD.WorkList)obj;
                if(msg.works.Length != 1)
                {
                    LOG.warn();
                    return;
                }
                mTask = msg.works[0];
                updateTaskInfo();
            }
            else if(DEF.CMD_ChatRoomInfo == obj.msgID)
            {
                ProcMsgList((ChatRoomInfo)obj);
            }
            else if (DEF.CMD_DelChatUsers == obj.msgID)
            {
                Close();
            }
        }

        private void ProcMsgList(ChatRoomInfo msg)
        {
            if (msg.body.mesgs == null)
                return;

            foreach (var chat in msg.body.mesgs.Reverse())
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

        private void updateTaskInfo()
        {
            btnWorker.Text = mTask.worker;
            btnDirector.Text = mTask.director;
            tbComment.Text = mTask.comment;
            tbTitle.Text = mTask.title;
            tbTerm.Text = mTask.term;
            tbDue.Text = mTask.due;
            tbLaunch.Text = mTask.launch;
            cbPriority.Text = mTask.priority;
            cbSubCate.Text = mTask.subCate;
            cbMainCate.Text = mTask.mainCate;
            cbAccess.Text = mTask.access;
            cbType.Text = mTask.type;
        }

        private void enableAllCtrls(bool isEn)
        {
            btnWorker.Enabled   = isEn;
            btnDirector.Enabled = isEn;
            tbComment.Enabled   = isEn;
            tbTitle.Enabled     = isEn;
            tbTerm.Enabled      = isEn;
            tbDue.Enabled       = isEn;
            tbLaunch.Enabled    = isEn;
            cbPriority.Enabled  = isEn;
            cbSubCate.Enabled   = isEn;
            cbMainCate.Enabled  = isEn;
            cbAccess.Enabled    = isEn;
            cbType.Enabled      = isEn;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            isEditable = !isEditable;

            enableAllCtrls(isEditable);

            btnReqFix.Visible = isEditable;
            btnEdit.Text = isEditable ? "취소" : "편집";
            if(!isEditable)
            {
                updateTaskInfo();
            }
        }

        private void sendEditInfo()
        {
            List<WorkHistory> vec = new List<WorkHistory>();
            if (cbMainCate.Text != mTask.mainCate)
            {
                WorkHistory item = new WorkHistory();
                item.taskID = mTask.recordID;
                item.editor = MyInfo.mMyInfo.userID;
                item.columnName = "mainCate";
                item.fromInfo = mTask.mainCate;
                item.toInfo = cbMainCate.Text;
                vec.Add(item);
            }
            if (cbSubCate.Text != mTask.subCate)
            {
                WorkHistory item = new WorkHistory();
                item.taskID = mTask.recordID;
                item.editor = MyInfo.mMyInfo.userID;
                item.columnName = "subCate";
                item.fromInfo = mTask.subCate;
                item.toInfo = cbSubCate.Text;
                vec.Add(item);
            }
            if (cbType.Text != mTask.type)
            {
                WorkHistory item = new WorkHistory();
                item.taskID = mTask.recordID;
                item.editor = MyInfo.mMyInfo.userID;
                item.columnName = "type";
                item.fromInfo = mTask.type;
                item.toInfo = cbType.Text;
                vec.Add(item);
            }
            if (tbTitle.Text != mTask.title)
            {
                WorkHistory item = new WorkHistory();
                item.taskID = mTask.recordID;
                item.editor = MyInfo.mMyInfo.userID;
                item.columnName = "title";
                item.fromInfo = mTask.title;
                item.toInfo = tbTitle.Text;
                vec.Add(item);
            }
            if (btnDirector.Text != mTask.director)
            {
                WorkHistory item = new WorkHistory();
                item.taskID = mTask.recordID;
                item.editor = MyInfo.mMyInfo.userID;
                item.columnName = "director";
                item.fromInfo = mTask.director;
                item.toInfo = btnDirector.Text;
                vec.Add(item);
            }
            if (btnWorker.Text != mTask.worker)
            {
                WorkHistory item = new WorkHistory();
                item.taskID = mTask.recordID;
                item.editor = MyInfo.mMyInfo.userID;
                item.columnName = "worker";
                item.fromInfo = mTask.worker;
                item.toInfo = btnWorker.Text;
                vec.Add(item);
            }
            //if (cbPriority.Text != mTask.priority) msg.info += string.Format("priority:{0},", cbPriority.Text);
            //if (cbAccess.Text != mTask.access) msg.info += string.Format("access:{0},", cbAccess.Text);
            //if (tbComment.Text != mTask.comment) msg.info += string.Format("comment:{0},", tbComment.Text);
            //if (tbLaunch.Text != mTask.launch) msg.info += string.Format("launch:{0},", tbLaunch.Text);
            //if (tbTerm.Text != mTask.term) msg.info += string.Format("term:{0},", tbTerm.Text);
            //if (tbDue.Text != mTask.due) msg.info += string.Format("term:{0},", tbDue.Text);

            if (vec.Count == 0)
            {
                MessageBox.Show("변경점이 없습니다.");
            }
            else
            {
                ICD.WorkHistoryList msg = new ICD.WorkHistoryList();
                msg.FillClientHeader(ICD.DEF.CMD_TaskEdit);
                msg.workHistory = vec.ToArray();
                ICDPacketMgr.GetInst().sendMsgToServer(msg);
            }
            
        }

        private void btnReqFix_Click(object sender, EventArgs e)
        {
            sendEditInfo();
            isEditable = false;
            btnReqFix.Visible = false;
            btnEdit.Text = "편집";
            updateTaskInfo();
            enableAllCtrls(false);
        }

        private void tbChatMsg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift == false && e.KeyCode == Keys.Enter)
            {
                SendChatMessage();
            }
        }

        private void SendChatMessage()
        {
            if (tbChatMsg.Text.Length == 0)
                return;

            ChatRoomInfo msg = new ChatRoomInfo();
            msg.FillClientHeader(DEF.CMD_ChatMsg, 0);
            msg.body.recordID = mTask.chatID;
            msg.body.mesgs = new MesgInfo[1];
            msg.body.mesgs[0] = new MesgInfo();
            msg.body.mesgs[0].message = tbChatMsg.Text;

            ICDPacketMgr.GetInst().sendMsgToServer(msg);
            tbChatMsg.Text = "";
        }

        private void InitListViews()
        {
            lvChatList.View = View.Details;
            lvChatList.GridLines = true;
            lvChatList.FullRowSelect = true;
            lvChatList.Sorting = SortOrder.None;
            lvChatList.Columns.Add("id");
            lvChatList.Columns[0].Width = 50;
            lvChatList.Columns.Add("user");
            lvChatList.Columns[1].Width = 100;
            lvChatList.Columns.Add("message");
            lvChatList.Columns[2].Width = 100;
            lvChatList.Columns.Add("state");
            lvChatList.Columns[3].Width = 50;
        }

        private void UpdateMsgListView(int id)
        {
            var msg = mMsgList[id];
            if (msg.index >= lvChatList.Items.Count)
            {
                string[] infos = new string[4];
                infos[0] = msg.msgID.ToString();
                infos[1] = msg.user;
                infos[2] = msg.message;
                infos[3] = msg.tick.ToString();
                ListViewItem pack = new ListViewItem(infos);
                lvChatList.Items.Add(pack);
            }
            else
            {
                var item = lvChatList.Items[msg.index];
                item.SubItems[0].Text = msg.msgID.ToString();
                item.SubItems[1].Text = msg.user;
                item.SubItems[2].Text = msg.message;
                item.SubItems[3].Text = msg.tick.ToString();
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            DlgEditChatUsers dlg = new DlgEditChatUsers(mTask.chatID);
            dlg.ShowDialog();

            string[] pre = dlg.GetUsers();
            string[] after = dlg.mChatNewList;

            if (pre == null || after == null)
                return;


            {
                ChatRoomInfo msg = new ChatRoomInfo();
                msg.FillClientHeader(DEF.CMD_DelChatUsers, 0);
                msg.body.recordID = mTask.chatID;
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
                msg.body.recordID = mTask.chatID;
                List<string> newUsers = new List<string>();
                foreach (string item in after)
                {
                    if (!pre.Contains(item))
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
            string me = MyInfo.mMyInfo.userID;
            if(mTask.director != me && mTask.worker != me)
            {
                ChatRoomInfo msg = new ChatRoomInfo();
                msg.FillClientHeader(DEF.CMD_DelChatUsers, 0);
                msg.body.recordID = mTask.chatID;
                msg.body.users = new string[1];
                msg.body.users[0] = me;
                ICDPacketMgr.GetInst().sendMsgToServer(msg);
            }
            Close();
        }
    }
}
