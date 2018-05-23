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
        
        public enum NetType
        {
            NONE,CONNECT,DISCON,DATA
        }

        public class QueuePack
        {
            public int ClientID = -1;
            public string ipAddr = "";
            public FifoBuffer buf = null;
            public NetType type = NetType.NONE;
        }

        private class ClientBuf
        {
            public int id { get; set; }
            public string address { get; set; }
            public TcpClient client { get; set; }
            public FifoBuffer buf { get; set; }
        }

        private const int PORTNUM                   = 7000;
        private const int MAX_CLIENT_CNT            = 1000;
        private const int MAX_SOCKET_BUF            = 1024;
        private const int MAX_CLIENT_BUF            = 1024*1024;


        private ClientBuf[]             mClients        = new ClientBuf[MAX_CLIENT_CNT];
        private ConcurrentQueue<QueuePack>  mQueue      = new ConcurrentQueue<QueuePack>();
        private ManualResetEvent            mMRE        = new ManualResetEvent(false);

        public event EventHandler mRecv = null;

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
                mRecv?.Invoke(mQueue, null);
            }
        }
        public void WriteToClient(int id, byte[] buf)
        {
            int idx = id;
            if (mClients[idx] == null)
                return;

            NetworkStream stream = mClients[idx].client.GetStream();
            stream.Write(buf, 0, buf.Length);
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
            mClients[idx].address = ((IPEndPoint)newClient.Client.RemoteEndPoint).Address.ToString();
            mClients[idx].buf = new FifoBuffer(MAX_CLIENT_BUF);

            QueuePack info = new QueuePack();
            info.ClientID = newID;
            info.buf = null;
            info.ipAddr = mClients[idx].address;
            info.type = NetType.CONNECT;
            mQueue.Enqueue(info);
            mMRE.Set();

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
                int nBytes = 0;
                try { 
                    nBytes = stream.Read(buff, 0, buff.Length);
                    if (nBytes == 0)
                        break;
                }
                catch(Exception e) { LOG.echo(e.ToString()); break; }

                clientPack.buf.Push(buff, nBytes);

                QueuePack info = new QueuePack();
                info.ClientID = clientPack.id;
                info.buf = clientPack.buf;
                info.type = NetType.DATA;
                info.ipAddr = clientPack.address;
                mQueue.Enqueue(info);

                mMRE.Set();
            }

            stream.Close();
            CloseClient(clientPack.id);
        }

        private void CloseClient(int id)
        {
            int idx = id;
            ClientBuf clientPack = mClients[idx];

            QueuePack info = new QueuePack();
            info.ClientID = id;
            info.buf = null;
            info.ipAddr = clientPack.address;
            info.type = NetType.DISCON;
            mQueue.Enqueue(info);
            mMRE.Set();

            clientPack.client.Close();
            clientPack.id = -1;
            clientPack.client = null;
            clientPack.buf = null;
            clientPack.address = "";
            mClients[idx] = null;

        }
    }
}
