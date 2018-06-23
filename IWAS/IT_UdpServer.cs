using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IWAS
{
    class IT_UdpServer
    {
        private static IT_UdpServer mInst_Singleton = new IT_UdpServer();
        private IT_UdpServer() { }
        public static IT_UdpServer GetInst() { return mInst_Singleton; }

        private const int PORTNUM = 7000;
        private const int MAX_FIFO_BUF = 1024 * 1024;

        private UdpClient mServer = new UdpClient();
        private Dictionary<string, FifoBuffer> mClients = new Dictionary<string, FifoBuffer>();

        public delegate void DelegateServer(string ipAddr, ICD.HEADER msg);
        public event DelegateServer OnProcRead;

        public void StartServer()
        {
            Task.Factory.StartNew(Run_waitClient, null);
        }

        public int Write(string ipAddr, byte[] buf)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ipAddr), PORTNUM);
            return mServer.Send(buf, buf.Length, remoteEP);
        }

        private void Run_waitClient(object o)
        {
            while (true)
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, PORTNUM);
                byte[] recvBuf = mServer.Receive(ref remoteEP);
                if (recvBuf == null || recvBuf.Length == 0)
                    break;

                string ipAddr = remoteEP.ToString();
                if(!mClients.ContainsKey(ipAddr))
                    mClients[ipAddr] = new FifoBuffer(MAX_FIFO_BUF);

                FifoBuffer fifoBuf = mClients[ipAddr];
                fifoBuf.Push(recvBuf, recvBuf.Length);

                while (true)
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
        }
    }
}
