using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IWAS
{
    class IT_TcpServer
    {
        private static IT_TcpServer mInst_Singleton = new IT_TcpServer();
        private IT_TcpServer() { }
        public static IT_TcpServer GetInst() { return mInst_Singleton; }

        private const int PORTNUM = 7000;
        private const int MAX_SOCKET_BUF = 1024;
        private const int MAX_FIFO_BUF = 1024 * 1024;

        Dictionary<string, TcpClient> mClients = new Dictionary<string, TcpClient>();

        public delegate void DelegateServer(string ipAddr, ICD.HEADER msg);
        public event DelegateServer OnProcConnect;
        public event DelegateServer OnProcDisConnect;
        public event DelegateServer OnProcRead;

        async public void StartServer()
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

        public void Write(string ipAddr, byte[] buf)
        {
            if ( !mClients.ContainsKey(ipAddr) )
                return;

            NetworkStream stream = mClients[ipAddr].GetStream();
            stream.Write(buf, 0, buf.Length);
        }

        private void createNewClient(TcpClient newClient)
        {
            string ipAddr = ((IPEndPoint)newClient.Client.RemoteEndPoint).Address.ToString();

            if ( mClients.ContainsKey(ipAddr) )
            {
                LOG.warn();
                CloseClient(ipAddr);
            }

            OnProcConnect?.Invoke(ipAddr, null);
            TcpClient client = new TcpClient();
            mClients[ipAddr] = client;

            Task.Factory.StartNew(Run_waitClient, client);
        }

        private void Run_waitClient(object o)
        {
            TcpClient client = (TcpClient)o;
            string ipAddr = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            NetworkStream stream = client.GetStream();
            FifoBuffer fifoBuf = new FifoBuffer(MAX_FIFO_BUF);

            byte[] buff = new byte[MAX_SOCKET_BUF];
            while (true)
            {
                int nBytes = 0;
                try
                {
                    nBytes = stream.Read(buff, 0, buff.Length);
                    if (nBytes == 0)
                        break;
                }
                catch (Exception e) { LOG.echo(e.ToString()); break; }

                fifoBuf.Push(buff, nBytes);

                while(true)
                {
                    int len = fifoBuf.GetSize();
                    byte[] buf = fifoBuf.readSize(len);
                    ICD.HEADER msg = ICD.HEADER.ConvertBytesToICDMessage(buf);
                    if (msg == null)
                        break;

                    fifoBuf.Pop(msg.msgSize);
                    OnProcRead?.Invoke(ipAddr, msg);
                }
                
            }

            fifoBuf.Clear();
            stream.Close();
            CloseClient(ipAddr);
        }

        private void CloseClient(string ipAddr)
        {
            if (!mClients.ContainsKey(ipAddr))
                return;

            TcpClient client = mClients[ipAddr];
            client.Close();
            mClients.Remove(ipAddr);
            OnProcDisConnect?.Invoke(ipAddr, null);
        }
    }
}
