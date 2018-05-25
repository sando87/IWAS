using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IWAS
{
    namespace ICD
    {
        public class DEF
        {
            public const int CMD_NONE           = 0;
            public const int CMD_NewUser        = 1;
            public const int CMD_UserList       = 2;
            public const int CMD_Login          = 3;
            public const int CMD_Logout         = 4;
            public const int CMD_TaskNew        = 5;
            public const int CMD_TaskEdit       = 6;
            public const int CMD_TaskDelete     = 7;
            public const int CMD_TaskList       = 8;
            public const int CMD_TaskInfo       = 9;
            //public const int CMD_NewChat        = 10;
            //public const int CMD_AddChat        = 11;
            public const int CMD_UploadFile     = 12;
            public const int CMD_DownloadFile   = 13;
            public const int CMD_LogMessage     = 14;
            public const int CMD_Search         = 15;
            public const int CMD_NewChat        = 16;
            public const int CMD_ChatMsg        = 17;
            public const int CMD_AddChatUser    = 18;
            public const int CMD_DelChatUser    = 19;
            public const int CMD_ShowChat       = 20;
            public const int CMD_HideChat       = 21;
            public const int CMD_ChatMsgList    = 22;
            public const int CMD_ChatUserList   = 23;
            public const int CMD_MAX_COUNT      = 24;

            public const int TYPE_REQ = 1;
            public const int TYPE_ACK = 2;
            public const int TYPE_REP = 3;
            public const int TYPE_ALT = 4;

            public const int MAGIC_SOF = 0xaa;
            public const int MAGIC_EOF = 0xbb;

            public const int ERR_NoError    = 0;
            public const int ERR_HaveID     = 1;
            public const int ERR_NoID       = 2;
            public const int ERR_WorngPW    = 3;
            
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int msgSOF;
            [MarshalAs(UnmanagedType.I4)] public int msgID;
            [MarshalAs(UnmanagedType.I4)] public int msgSize;
            [MarshalAs(UnmanagedType.I4)] public int msgType;
            [MarshalAs(UnmanagedType.I4)] public int msgErr;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string msgUser;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string msgTime;

            public void FillClientHeader(int id)
            {
                msgID = id;
                msgSize = Marshal.SizeOf(this);
                msgSOF = DEF.MAGIC_SOF;
                msgType = DEF.TYPE_REQ;
                msgErr = DEF.ERR_NoError;
                msgUser = MyInfo.mMyInfo.userID;
                msgTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            public void FillHeader(int id, int type, string user)
            {
                msgID = id;
                msgSize = Marshal.SizeOf(this);
                msgSOF = DEF.MAGIC_SOF;
                msgType = type;
                msgErr = DEF.ERR_NoError;
                msgUser = user;
                msgTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            public void FillServerHeader(int id)
            {
                msgID = id;
                msgSize = Marshal.SizeOf(this);
                msgSOF = DEF.MAGIC_SOF;
                msgType = DEF.TYPE_REQ;
                msgErr = DEF.ERR_NoError;
                msgUser = ConstDefines.SYSNAME;
                msgTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            static public int HeaderSize()
            {
                return Marshal.SizeOf(typeof(HEADER));
            }

            public byte[] Serialize()
            {
                // allocate a byte array for the struct data
                var buffer = new byte[Marshal.SizeOf(this)];

                // Allocate a GCHandle and get the array pointer
                var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                var pBuffer = gch.AddrOfPinnedObject();

                // copy data from struct to array and unpin the gc pointer
                Marshal.StructureToPtr(this, pBuffer, false);
                gch.Free();

                return buffer;
            }

            public void Deserialize(ref byte[] data)
            {
                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                Marshal.PtrToStructure(gch.AddrOfPinnedObject(), this);
                gch.Free();
            }

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class User : HEADER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string userID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string userPW;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Task : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int cmdID;
            [MarshalAs(UnmanagedType.I4)] public int progress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string createTime;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string kind;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string access;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string mainCategory;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string subCategory;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string title;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string comment;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string creator;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string director;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string worker;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string preLaunch;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string preterm;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string preDue;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string state;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string priority;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class TaskEdit : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int taskID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string info;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Chat : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int state;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string access;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string info;
        }

        public class ChatMsgs : HEADER
        {
            public int msgCount;
            public ChatMsg[] msgs;

            public new byte[] Serialize()
            {
                List<byte> ary = new List<byte>();
                HEADER head = new HEADER();

                head.msgSOF = msgSOF;
                head.msgID = msgID;
                head.msgSize = msgSize;
                head.msgType = msgType;
                head.msgErr = msgErr;
                head.msgUser = msgUser;
                head.msgTime = msgTime;

                ary.AddRange(head.Serialize());

                ary.AddRange(BitConverter.GetBytes(msgCount));

                for(int i=0; i<msgCount; ++i)
                {
                    int msgSize = Marshal.SizeOf(typeof(ChatMsg));
                    byte[] src = new byte[msgSize];
                    var gch = GCHandle.Alloc(src, GCHandleType.Pinned);
                    var pBuffer = gch.AddrOfPinnedObject();
                    Marshal.StructureToPtr(msgs[i], pBuffer, false);
                    gch.Free();
                    ary.AddRange(src);
                }

                return ary.ToArray();
            }

            public new void Deserialize(ref byte[] data)
            {
                msgSOF = BitConverter.ToInt32(data, 0);
                msgID = BitConverter.ToInt32(data, 4);
                msgSize = BitConverter.ToInt32(data, 8);
                msgType = BitConverter.ToInt32(data, 12);
                msgErr = BitConverter.ToInt32(data, 16);

                msgUser = Encoding.ASCII.GetString(data, 20, 50).TrimEnd('\0');
                msgTime = Encoding.ASCII.GetString(data, 70, 50).TrimEnd('\0');

                int headLen = HEADER.HeaderSize();
                msgCount = BitConverter.ToInt32(data, headLen);
                msgs = new ChatMsg[msgCount];
                for (int i=0; i<msgCount; ++i)
                {
                    int msgSize = Marshal.SizeOf(typeof(ChatMsg));
                    byte[] dest = new byte[msgSize];
                    int off = headLen + 4 + (i * msgSize);
                    Array.Copy(data, off, dest, 0, msgSize);

                    msgs[i] = new ChatMsg();
                    var gch = GCHandle.Alloc(dest, GCHandleType.Pinned);
                    Marshal.PtrToStructure(gch.AddrOfPinnedObject(), msgs[i]);
                    gch.Free();
                }
                
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class ChatMsg
        {
            [MarshalAs(UnmanagedType.I4)] public int msgID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string time;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string user;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string message;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class File : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int filsSize;
            [MarshalAs(UnmanagedType.I4)] public int chatID;
            [MarshalAs(UnmanagedType.I4)] public int packetIDX;
            [MarshalAs(UnmanagedType.I4)] public int packetCNT;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string createTime;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string fileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string fileDir;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string ext;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string user;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] data;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Message : HEADER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string message;

        }
    }
}
