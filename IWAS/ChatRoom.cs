using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IWAS.ICD;

namespace IWAS
{
    class ChatRoom
    {
        class MsgInfo
        {
            int msgID;
            int tick;
            string time;
            string user;
            string message;
        }

        class UserState
        {
            bool isON;
            int idxMsg;
            string user;
        }

        int roomID;
        Dictionary<string, UserState> mUsers = new Dictionary<string, UserState>();
        List<MsgInfo> mMessages = new List<MsgInfo>();

        public void Init(int chatID)
        {
            roomID = chatID;
            //load chat messages from DB chatID
            //fill mUsers, mMessages
        }
        public void ProcChat(Chat obj)
        {
            switch(obj.command)
            {
                case DEF.CHAT_CMD_NewChat:
                    ProcNewChat(obj);
                    break;
                case DEF.CHAT_CMD_Message:
                    ProcMessage(obj);
                    break;
                case DEF.CHAT_CMD_NewUser:
                    ProcNewUser(obj);
                    break;
                case DEF.CHAT_CMD_DelUser:
                    ProcDelUser(obj);
                    break;
                case DEF.CHAT_CMD_InUser:
                    ProcInUser(obj);
                    break;
                case DEF.CHAT_CMD_OutUser:
                    ProcOutUser(obj);
                    break;
                default:
                    LOG.warn();
                    break;
            }
        }
        private void ProcNewChat(Chat obj)
        {

        }
        private void ProcMessage(Chat obj)
        {

        }
        private void ProcNewUser(Chat obj)
        {

        }
        private void ProcDelUser(Chat obj)
        {

        }
        private void ProcInUser(Chat obj)
        {

        }
        private void ProcOutUser(Chat obj)
        {

        }
    }
}
