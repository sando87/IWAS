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
        private HEADER CreateIcdObject(HEADER head)
        {
            int msgSize = head.msgSize;
            if((head.msgID/10) == 2) //ID가 20~29이면
            {
                return new ChatRoomInfo();
            }
            else if (head.msgID == DEF.CMD_ChatRoomList)
            {
                return new ChatRoomList(1);
            }
            else if (msgSize == Marshal.SizeOf(typeof(HEADER)))
            {
                return new HEADER();
            }
            else if (msgSize == Marshal.SizeOf(typeof(User)))
            {
                return new User();
            }
            else if (msgSize == Marshal.SizeOf(typeof(ICD.Task)))
            {
                return new ICD.Task();
            }
            else if (msgSize == Marshal.SizeOf(typeof(TaskEdit)))
            {
                return new TaskEdit();
            }
            else if (msgSize == Marshal.SizeOf(typeof(File)))
            {
                return new File();
            }
            else if (msgSize == Marshal.SizeOf(typeof(Message)))
            {
                return new Message();
            }
            else
            {
                LOG.warn();
                return new HEADER();
            }
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
                HEADER msg = CreateIcdObject(head);
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
