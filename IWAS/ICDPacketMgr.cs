﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWAS.ICD;
using System.Collections.Concurrent;

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

        public void StartServiceServer()
        {
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
        private HEADER CreateIcdObject(COMMAND id)
        {
            switch (id)
            {
                case COMMAND.NewUser:
                    return new ICD.User();
                case COMMAND.TaskNew:
                    return new ICD.Task();
                case COMMAND.NewChat:
                    return new ICD.Chat();
                case COMMAND.UploadFile:
                    return new ICD.File();
                case COMMAND.LogMessage:
                    return new ICD.Message();
                default:
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
            OnConnected.Invoke(pack.ClientID, null);
        }

        private void procDisConnect(NetworkMgr.QueuePack pack)
        {
            OnDisConnected.Invoke(pack.ClientID, null);
        }

        private void procData(NetworkMgr.QueuePack pack)
        {
            long nRecvLen = pack.buf.GetSize();
            int headSize = ICD.HEADER.HeaderSize();
            if (nRecvLen < headSize)
                return;

            byte[] headBuf = pack.buf.readSize(headSize);
            HEADER head = new HEADER();
            head.Deserialize(ref headBuf);
            if (head.msgSOF != (uint)ICD.MAGIC.SOF)
            {
                pack.buf.Clear();
                return;
            }

            uint msgSize = head.msgSize;
            if (nRecvLen < msgSize)
                return;

            byte[] msgBuf = pack.buf.Pop((int)msgSize);
            HEADER msg = CreateIcdObject((COMMAND)head.msgID);
            msg.Deserialize(ref msgBuf);
            OnRecv.Invoke(pack.ClientID, msg);
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
