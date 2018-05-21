using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace IWAS
{
    class NetworkMgr
    {
        private static NetworkMgr SingletonInstance = new NetworkMgr();
        private NetworkMgr() { }
        public static NetworkMgr GetInst() { return SingletonInstance; }
        

        public class QueuePack
        {
            public int ClientID = -1;
            public byte[] buf = null;
            public uint size = 0;
        }

        private class ClientBuf
        {
            public int id { get; set; }
            public TcpClient client { get; set; }
            public FifoBuffer buf { get; set; }
            public string userName { get; set; }
        }

        private const int PORTNUM                   = 7000;
        private const int MAX_CLIENT_CNT            = 1000;
        private const int MAX_SOCKET_BUF            = 1024;
        private const int MAX_CLIENT_BUF            = 1024*1024;


        private Dictionary<string, int> mUserNameMap    = new Dictionary<string, int>();
        private ClientBuf[]             mClients        = new ClientBuf[MAX_CLIENT_CNT];
        private ConcurrentQueue<QueuePack>  mQueue      = new ConcurrentQueue<QueuePack>();
        private ManualResetEvent            mMRE        = new ManualResetEvent(false);

        public event EventHandler mRecv = null;
        public event EventHandler mConn = null;

        async public void acceptAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORTNUM);
            listener.Start();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                if (client == null)
                    continue;

                createNewClient(client);
            }

        }
        public int connectServer(string ip = "127.0.0.1", int port = PORTNUM)
        {
            TcpClient client = new TcpClient(ip, port);

            int clientID = createNewClient(client);
            return clientID;
        }
        async public void startAsync()
        {
            while(true)
            {
                Task task = Task.Run(() => { mMRE.WaitOne(); });
                await task;
                mMRE.Reset();
                mRecv.Invoke(mQueue, null);
            }
        }
        public void WriteToServer(byte[] buf)
        {
            int idx = 0;
            if (mClients[idx] == null)
                return;

            NetworkStream stream = mClients[idx].client.GetStream();
            stream.Write(buf, 0, buf.Length);
        }
        public void WriteToClient(int id, byte[] buf)
        {
            int idx = id;
            if (mClients[idx] == null)
                return;

            NetworkStream stream = mClients[idx].client.GetStream();
            stream.Write(buf, 0, buf.Length);
        }
        public void WriteToClient(string user, byte[] buf)
        {
            if ( !mUserNameMap.ContainsKey(user) )
                return;

            int id = mUserNameMap[user];
            NetworkStream stream = mClients[id].client.GetStream();
            stream.Write(buf, 0, buf.Length);
        }
        public void LoginUser(int id, string UserName)
        {
            int idx = id;
            if (mClients[idx] == null)
                return;

            mClients[idx].userName = UserName;
            mUserNameMap[UserName] = id;
        }
        public void LogoutUser(string UserName)
        {
            if (!mUserNameMap.ContainsKey(UserName))
                return;

            int id = mUserNameMap[UserName];
            mClients[id].userName = null;
            mUserNameMap.Remove(UserName);
        }

        private int findEmptyID()
        {
            for (int idx = 0; idx < mClients.Length; idx++)
            {
                if (mClients[idx] == null)
                    return idx;
            }
            return -1;
        }

        private int createNewClient(TcpClient newClient)
        {
            int newID = findEmptyID();
            if (newID == -1)
                return -1;

            int idx = newID;
            mClients[idx] = new ClientBuf();
            mClients[idx].id = newID;
            mClients[idx].client = newClient;
            mClients[idx].buf = new FifoBuffer(MAX_CLIENT_BUF);
            mClients[idx].userName = null;

            Task.Factory.StartNew(waitClient, mClients[idx]);
            return newID;
        }

        private void waitClient(object o)
        {
            ClientBuf clientPack = (ClientBuf)o;

            NetworkStream stream = clientPack.client.GetStream();

            byte[] buff = new byte[MAX_SOCKET_BUF];
            while(true)
            {
                int nBytes = stream.Read(buff, 0, buff.Length);
                if (nBytes == 0)
                    break;

                clientPack.buf.Push(buff, nBytes);
                if(pushPacket(ref clientPack))
                {
                    mMRE.Set();
                }
            }


            stream.Close();
            CloseClient(clientPack.id);
        }

        private bool pushPacket(ref ClientBuf client)
        {
            long nRecvLen = client.buf.GetSize();
            int headSize = ICD.HEADER.HeaderSize();
            if (nRecvLen < headSize)
                return false;

            byte[] headBuf = client.buf.readSize(headSize);
            ICD.HEADER head = ICD.HEADER.GetHeaderInfo(headBuf);
            if (head.msgSOF != (uint)ICD.MAGIC.SOF)
            {
                client.buf.Clear();
                return false;
            }

            uint msgSize = head.msgSize;
            if (nRecvLen < msgSize)
                return false;

            QueuePack pack = new QueuePack();
            pack.ClientID = client.id;
            pack.size = msgSize;
            pack.buf = client.buf.Pop((int)msgSize);
            mQueue.Enqueue(pack);

            return true;
        }

        private void CloseClient(int id)
        {
            int idx = id;
            ClientBuf clientPack = mClients[idx];
            clientPack.client.Close();

            if (clientPack.userName != null)
                mUserNameMap.Remove(clientPack.userName);

            clientPack.id = -1;
            clientPack.client = null;
            clientPack.buf = null;
            clientPack.userName = null;
            mClients[idx] = null;
        }
    }
}
