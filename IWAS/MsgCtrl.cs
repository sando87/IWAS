using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWAS.ICD;

namespace IWAS
{
    class MsgCtrl
    {
        static private MsgCtrl mInst = null;
        static public MsgCtrl GetInst() { if (mInst == null) mInst = new MsgCtrl(); return mInst; }
        private MsgCtrl() { }

        ICDPacketMgr.PacketHandler[] mFuncArray = null;

        public void StartService()
        {
            if (mFuncArray != null)
                return;

            mFuncArray = new ICDPacketMgr.PacketHandler[(uint)COMMAND.MAX_COUNT];
            mFuncArray[(uint)COMMAND.NewUser] = ICD_NewUser;
            mFuncArray[(uint)COMMAND.Login] = ICD_Login;
            mFuncArray[(uint)COMMAND.Logout] = ICD_Logout;

            ICDPacketMgr.GetInst().OnRecv += (id, obj) =>
            {
                if (mFuncArray != null)
                    mFuncArray[obj.msgID]?.Invoke(id, obj);
            };
        }

        private void ICD_NewUser(int clientID, HEADER obj)
        {
            User msg = obj as User;

            if (DatabaseMgr.GetUserInfo(msg.userID) != null)
            {
                //send back error msg : same user id
            }
            else
            {
                //push db new user
                //ack good
            }
        }
        private void ICD_Login(int clientID, HEADER obj)
        {
            User msg = obj as User;
            DataRow row = DatabaseMgr.GetUserInfo(msg.userID);
            if (row != null)
            {
                //if(isOKpassword)
                {
                    //loginUser();
                    //ack good
                }
                //else
                {
                    //send back error msg : wrong password
                }
            }
            else
            {
                //send back error msg : no user id
            }

        }
        private void ICD_Logout(int clientID, HEADER obj)
        {

        }
    }
}
