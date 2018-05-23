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
    public partial class Login : Form
    {
        private const string MSG_SUCCESS_NEWID = "회원가입 성공";
        private const string MSG_ALREADY_ID = "이미 등록된 ID입니다";
        private const string MSG_SUCCESS_LOGIN = "로그인 성공";
        private const string MSG_UNREGIST_ID = "등록되지 않은 ID입니다";
        private const string MSG_WRONG_PW = "잘못된 비밀번호입니다";
        private const string MSG_DIFF_PW = "비밀번호가 서로 다릅니다";
        private const string BTN_NEWUSER = "회원가입";
        private const string BTN_LOGIN = "로그인";


        private const string IPADDR = "127.0.0.1";
        private const int PORTNUM = 7000;

        public Login()
        {
            InitializeComponent();
        }

        public void ConnectServer()
        {

            ICDPacketMgr.GetInst().OnRecv += OnRecv_ICDMessages;

            //ICDPacketMgr.GetInst().StartServiceServer();

            ICDPacketMgr.GetInst().StartServiceClient(IPADDR, PORTNUM);
        }

        private void OnRecv_ICDMessages(int clientID, ICD.HEADER o)
        {
            ICD.HEADER obj = o as ICD.HEADER;
            switch ((ICD.COMMAND)obj.msgID)
            {
                case ICD.COMMAND.NewUser:
                    OnRecv_NewUser(obj);
                    break;
                case ICD.COMMAND.Login:
                    OnRecv_Login(obj);
                    break;
                default:
                    break;
            }
        }

        private void OnRecv_NewUser(ICD.HEADER obj)
        {
            ICD.ERRORCODE curErr = (ICD.ERRORCODE)obj.msgErr;
            switch (curErr)
            {
                case ICD.ERRORCODE.NOERROR:
                    MessageBox.Show(MSG_SUCCESS_NEWID);
                    break;
                case ICD.ERRORCODE.HaveID:
                    MessageBox.Show(MSG_ALREADY_ID);
                    break;
                default:
                    break;
            }
        }
        private void OnRecv_Login(ICD.HEADER obj)
        {
            ICD.ERRORCODE curErr = (ICD.ERRORCODE)obj.msgErr;
            switch (curErr)
            {
                case ICD.ERRORCODE.NOERROR:
                    MessageBox.Show(MSG_SUCCESS_LOGIN);
                    {
                        MyInfo.mMyInfo = obj as ICD.User;
                        ICDPacketMgr.GetInst().OnRecv -= OnRecv_ICDMessages;
                        Visible = false;

                        MyTasks task = new MyTasks();
                        task.ShowDialog();
                        Visible = true;

                    }
                    break;
                case ICD.ERRORCODE.NoID:
                    MessageBox.Show(MSG_UNREGIST_ID);
                    break;
                case ICD.ERRORCODE.WorngPW:
                    MessageBox.Show(MSG_WRONG_PW);
                    break;
                default:
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNewUser.Checked)
            {
                btnLogin.Text = BTN_NEWUSER;
                edPasswordCheck.Enabled = true;
            }
            else
            {
                btnLogin.Text = BTN_LOGIN;
                edPasswordCheck.Enabled = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbNewUser.Checked)
            {
                //회원가입 요청
                if (edPassword.Text == edPasswordCheck.Text)
                {
                    ICD.User obj = new ICD.User();
                    obj.FillClientHeader(ICD.COMMAND.NewUser);
                    obj.userID = edUserID.Text;
                    obj.userPW = edPassword.Text;
                    ICDPacketMgr.GetInst().sendMsgToServer(obj);
                }
                else
                {
                    MessageBox.Show(MSG_DIFF_PW);
                }

            }
            else
            {
                //로그인 요청
                ICD.User obj = new ICD.User();
                obj.FillClientHeader(ICD.COMMAND.Login);
                obj.userID = edUserID.Text;
                obj.userPW = edPassword.Text;
                ICDPacketMgr.GetInst().sendMsgToServer(obj);
            }

        }

    }
}
