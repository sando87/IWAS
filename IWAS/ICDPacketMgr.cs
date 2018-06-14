using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWAS.ICD;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace IWAS
{
    class ICDPacketMgr
    {
        //Singleton Class
        private static ICDPacketMgr mInst = null;
        private ICDPacketMgr() { }
        public static ICDPacketMgr GetInst() { if (mInst == null) { mInst = new ICDPacketMgr(); } return mInst; }

        //Event Handler
        public delegate void PacketHandler(int clientID, HEADER obj);
        public event PacketHandler OnRecv;
        public event PacketHandler OnDisConnected;
        public event PacketHandler OnConnected;
        private bool isRunServer = false;

        public void StartServiceServer()
        {
            if (isRunServer)
                return;

            isRunServer = true;
            NetworkMgr networkMgr = NetworkMgr.GetInst();
            networkMgr.mRecv += new EventHandler(OnRecvPacket);
            networkMgr.acceptAsync();
            networkMgr.startAsync();
        }
        public void StartServiceClient(string ip, int port)
        {
            NetworkMgr networkMgr = NetworkMgr.GetInst();
            networkMgr.mRecv += new EventHandler(OnRecvPacket);
            networkMgr.connectServer(ip, port);
            networkMgr.startAsync();
        }
        private HEADER CreateICD_onClient(HEADER head)
        {
            switch(head.msgID)
            {
                case DEF.CMD_TaskIDList:
                case DEF.CMD_TaskLatestInfo:
                case DEF.CMD_TaskBaseList:
                    return new WorkList();
                case DEF.CMD_TaskHistory:
                    return new WorkHistoryList();
                case DEF.CMD_ChatRoomList:
                    return new ChatRoomList(1);
                case DEF.CMD_NewChat:
                case DEF.CMD_ChatMsg:
                case DEF.CMD_AddChatUsers:
                case DEF.CMD_DelChatUsers:
                case DEF.CMD_ShowChat:
                case DEF.CMD_HideChat:
                case DEF.CMD_ChatRoomInfo:
                case DEF.CMD_ChatMsgAll:
                    return new ChatRoomInfo();
                case DEF.CMD_NewUser:
                case DEF.CMD_Logout:
                    return new HEADER();
                case DEF.CMD_UserList:
                    return new Message();
                case DEF.CMD_Login:
                    return new User();
            }
            LOG.warn();
            return null;
        }
        private HEADER CreateICD_onServer(HEADER head)
        {
            switch (head.msgID)
            {
                case DEF.CMD_TaskNew:
                    return new WorkList();
                case DEF.CMD_TaskEdit:
                    return new WorkHistoryList();
                case DEF.CMD_TaskBaseList:
                case DEF.CMD_TaskHistory:
                case DEF.CMD_TaskIDList:
                case DEF.CMD_ChatRoomList:
                case DEF.CMD_UserList:
                    return new HEADER();
                case DEF.CMD_NewChat:
                case DEF.CMD_ChatMsg:
                case DEF.CMD_AddChatUsers:
                case DEF.CMD_DelChatUsers:
                case DEF.CMD_ShowChat:
                case DEF.CMD_HideChat:
                case DEF.CMD_ChatRoomInfo:
                case DEF.CMD_ChatMsgAll:
                    return new ChatRoomInfo();
                case DEF.CMD_NewUser:
                case DEF.CMD_Login:
                    return new User();
            }
            LOG.warn();
            return null;
        }

        private void OnRecvPacket(object sender, EventArgs e)
        {
            var queue = (ConcurrentQueue<NetworkMgr.QueuePack>)sender;
            while(true)
            {
                NetworkMgr.QueuePack pack = null;
                if (queue.TryDequeue(out pack))
                {
                    switch(pack.type)
                    {
                        case NetworkMgr.NetType.CONNECT:
                            procConnect(pack);
                            break;
                        case NetworkMgr.NetType.DISCON:
                            procDisConnect(pack);
                            break;
                        case NetworkMgr.NetType.DATA:
                            procData(pack);
                            break;
                        default:
                            LOG.warn();
                            break;
                    }
                    
                }

                if (queue.IsEmpty)
                    break;
            }
        }

        private void procConnect(NetworkMgr.QueuePack pack)
        {
            OnConnected?.Invoke(pack.ClientID, null);
        }

        private void procDisConnect(NetworkMgr.QueuePack pack)
        {
            OnDisConnected?.Invoke(pack.ClientID, null);
        }

        private void procData(NetworkMgr.QueuePack pack)
        {
            while(true)
            {
                long nRecvLen = pack.buf.GetSize();
                int headSize = ICD.HEADER.HeaderSize();
                if (nRecvLen < headSize)
                    break;

                byte[] headBuf = pack.buf.readSize(headSize);
                HEADER head = new HEADER();
                head.Deserialize(ref headBuf);
                if (head.msgSOF != ICD.DEF.MAGIC_SOF)
                {
                    pack.buf.Clear();
                    break;
                }

                int msgSize = head.msgSize;
                if (nRecvLen < msgSize)
                    break;

                byte[] msgBuf = pack.buf.Pop(msgSize);

                HEADER msg = null;
                if (head.msgType == DEF.TYPE_REQ)
                    msg = CreateICD_onServer(head);
                else if (head.msgType == DEF.TYPE_REP)
                    msg = CreateICD_onClient(head);
                else
                    LOG.warn();

                msg.Deserialize(ref msgBuf);
                OnRecv?.Invoke(pack.ClientID, msg);
            }
        }

        public void sendMsgToServer(ICD.HEADER obj)
        {
            byte[] buf = obj.Serialize();
            NetworkMgr.GetInst().WriteToClient(0, buf);
        }

        public void sendMsgToClient(int clientID, ICD.HEADER obj)
        {
            byte[] buf = obj.Serialize();
            NetworkMgr.GetInst().WriteToClient(clientID, buf);
        }
        
    }
    
}
