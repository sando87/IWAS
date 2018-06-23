using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IWAS
{
    namespace ICD
    {
        public class DEF
        {
            public const int CMD_NONE = 0;
            public const int CMD_NewUser = 1;
            public const int CMD_UserList = 2;
            public const int CMD_Login = 3;
            public const int CMD_Logout = 4;
            //public const int CMD_NewChat        = 10;
            //public const int CMD_AddChat        = 11;
            //public const int CMD_UploadFile = 12;
            //public const int CMD_DownloadFile = 13;
            //public const int CMD_LogMessage = 14;
            //public const int CMD_Search = 15;

            public const int CMD_NewChat = 20;
            public const int CMD_ChatMsg = 21;
            public const int CMD_AddChatUsers = 22;
            public const int CMD_DelChatUsers = 23;
            public const int CMD_ShowChat = 24;
            public const int CMD_HideChat = 25;
            public const int CMD_ChatRoomInfo = 26;
            public const int CMD_ChatMsgAll = 27;
            //public const int CMD_ChatAddTask    = 28;
            //public const int CMD_ChatDelTask    = 29;
            public const int CMD_ChatRoomList = 30;

            public const int CMD_TaskBaseList = 40;
            public const int CMD_TaskHistory = 41;
            public const int CMD_TaskNew = 42;
            public const int CMD_TaskEdit = 43;
            public const int CMD_TaskIDList = 45;
            public const int CMD_TaskLatestInfo = 46;

            public const int CMD_MAX_COUNT = 50;

            public const int TYPE_NONE = 0;
            public const int TYPE_REQ = 1;
            public const int TYPE_ACK = 2;
            public const int TYPE_REP = 3;
            public const int TYPE_ALT = 4;

            public const int MAGIC_SOF = 0xaa;
            public const int MAGIC_EOF = 0xbb;

            public const int ERR_NoError = 0;
            public const int ERR_HaveID = 1;
            public const int ERR_NoID = 2;
            public const int ERR_WorngPW = 3;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int msgSOF;
            [MarshalAs(UnmanagedType.I4)] public int msgID;
            [MarshalAs(UnmanagedType.I4)] public int msgSize;
            [MarshalAs(UnmanagedType.I4)] public int msgType;
            [MarshalAs(UnmanagedType.I4)] public int msgErr;
            [MarshalAs(UnmanagedType.I8)] public long msgTime;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string msgUser;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string ext1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string ext2;

            public void FillClientHeader(int id, int size = -1)
            {
                msgID = id;
                msgSize = (size == -1) ? Marshal.SizeOf(this) : size;
                msgSOF = DEF.MAGIC_SOF;
                msgType = DEF.TYPE_REQ;
                msgErr = DEF.ERR_NoError;
                msgUser = MyInfo.mMyInfo.userID;
                msgTime = DateTime.Now.Ticks;
                ext1 = "";
                ext2 = "";
            }
            public void FillHeader(HEADER head)
            {
                msgID = head.msgID;
                msgSize = head.msgSize;
                msgSOF = head.msgSOF;
                msgType = head.msgType;
                msgErr = head.msgErr;
                msgUser = head.msgUser;
                msgTime = head.msgTime;
                ext1 = "";
                ext2 = "";
            }
            public void FillHeader(int id)
            {
                msgID = id;
                msgSize = HeaderSize();
                msgSOF = DEF.MAGIC_SOF;
                msgType = DEF.TYPE_NONE;
                msgErr = DEF.ERR_NoError;
                msgUser = ConstDefines.SYSNAME;
                msgTime = DateTime.Now.Ticks;
                ext1 = "";
                ext2 = "";
            }
            public void FillServerHeader(int id, int size = -1)
            {
                msgID = id;
                msgSize = (size == -1) ? Marshal.SizeOf(this) : size;
                msgSOF = DEF.MAGIC_SOF;
                msgType = DEF.TYPE_REP;
                msgErr = DEF.ERR_NoError;
                msgUser = ConstDefines.SYSNAME;
                msgTime = DateTime.Now.Ticks;
                ext1 = "";
                ext2 = "";
            }
            static public int HeaderSize()
            {
                return Marshal.SizeOf(typeof(HEADER));
            }

            public HEADER Clone()
            {
                HEADER newObj = new HEADER();

                newObj.msgID = msgID;
                newObj.msgSize = msgSize;
                newObj.msgSOF = msgSOF;
                newObj.msgType = msgType;
                newObj.msgErr = msgErr;
                newObj.msgUser = msgUser;
                newObj.msgTime = msgTime;
                newObj.ext1 = ext1;
                newObj.ext2 = ext2;

                return newObj;
            }

            public virtual byte[] Serialize()
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

            public virtual void Deserialize(ref byte[] data)
            {
                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                Marshal.PtrToStructure(gch.AddrOfPinnedObject(), this);
                gch.Free();
            }

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class User : HEADER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string userID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string userPW;

        }
        /*
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class Task : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int cmdID;
            [MarshalAs(UnmanagedType.I4)] public int progress;
            [MarshalAs(UnmanagedType.I4)] public int chatID;
            [MarshalAs(UnmanagedType.I4)] public int currentState;
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class TaskEdit : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int taskID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string info;

        }
        */

        /*
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class Chat : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int state;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string access;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string info;
        }
        */
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
                head.ext1 = ext1;
                head.ext2 = ext2;

                ary.AddRange(head.Serialize());

                ary.AddRange(BitConverter.GetBytes(msgCount));

                for (int i = 0; i < msgCount; ++i)
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
                msgTime = BitConverter.ToInt64(data, 20);

                msgUser = Encoding.ASCII.GetString(data, 28, 58).TrimEnd('\0');
                ext1 = Encoding.ASCII.GetString(data, 78, 58).TrimEnd('\0');
                ext2 = Encoding.ASCII.GetString(data, 128, 58).TrimEnd('\0');

                int headLen = HEADER.HeaderSize();
                msgCount = BitConverter.ToInt32(data, headLen);
                msgs = new ChatMsg[msgCount];
                for (int i = 0; i < msgCount; ++i)
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ChatMsg
        {
            [MarshalAs(UnmanagedType.I4)] public int msgID;
            [MarshalAs(UnmanagedType.I8)] public long time;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string user;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string message;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class File : HEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int recordID;
            [MarshalAs(UnmanagedType.I4)] public int filsSize;
            [MarshalAs(UnmanagedType.I4)] public int chatID;
            [MarshalAs(UnmanagedType.I4)] public int packetIDX;
            [MarshalAs(UnmanagedType.I4)] public int packetCNT;
            [MarshalAs(UnmanagedType.I8)] public long createTime;
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class Message : HEADER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string message;

        }

        public class ChatRoomInfo : HEADER
        {
            public RoomInfo body;
            public ChatRoomInfo()
            {
                body = new RoomInfo();
            }

            public override byte[] Serialize()
            {
                List<byte> ary = new List<byte>();

                byte[] bodyBuf = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, body);
                    bodyBuf = ms.ToArray();
                }

                HEADER head = Clone();
                head.msgSize = HeaderSize() + bodyBuf.Length;
                ary.AddRange(head.Serialize());
                ary.AddRange(bodyBuf);

                return ary.ToArray();
            }

            public override void Deserialize(ref byte[] data)
            {
                int headSize = HEADER.HeaderSize();
                int bodySize = data.Length - headSize;

                byte[] headBuf = new byte[headSize];
                Array.Copy(data, 0, headBuf, 0, headSize);
                HEADER head = new HEADER();
                head.Deserialize(ref headBuf);

                msgID = head.msgID;
                msgSize = head.msgSize;
                msgSOF = head.msgSOF;
                msgType = head.msgType;
                msgErr = head.msgErr;
                msgUser = head.msgUser;
                msgTime = head.msgTime;
                ext1 = head.ext1;
                ext2 = head.ext2;

                byte[] bodyBuf = new byte[bodySize];
                Array.Copy(data, headSize, bodyBuf, 0, bodySize);
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(bodyBuf))
                {
                    body = (RoomInfo)bf.Deserialize(ms);
                }
            }
        }

        public class ChatRoomList : HEADER
        {
            public RoomInfo[] body;
            public ChatRoomList(int n)
            {
                body = new RoomInfo[n];
            }

            public override byte[] Serialize()
            {
                List<byte> ary = new List<byte>();

                byte[] bodyBuf = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, body);
                    bodyBuf = ms.ToArray();
                }

                HEADER head = Clone();
                head.msgSize = HeaderSize() + bodyBuf.Length;
                ary.AddRange(head.Serialize());
                ary.AddRange(bodyBuf);

                return ary.ToArray();
            }

            public override void Deserialize(ref byte[] data)
            {
                int headSize = HEADER.HeaderSize();
                int bodySize = data.Length - headSize;

                byte[] headBuf = new byte[headSize];
                Array.Copy(data, 0, headBuf, 0, headSize);
                HEADER head = new HEADER();
                head.Deserialize(ref headBuf);

                msgID = head.msgID;
                msgSize = head.msgSize;
                msgSOF = head.msgSOF;
                msgType = head.msgType;
                msgErr = head.msgErr;
                msgUser = head.msgUser;
                msgTime = head.msgTime;
                ext1 = head.ext1;
                ext2 = head.ext2;

                byte[] bodyBuf = new byte[bodySize];
                Array.Copy(data, headSize, bodyBuf, 0, bodySize);
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(bodyBuf))
                {
                    body = (RoomInfo[])bf.Deserialize(ms);
                }
            }
        }

        [Serializable]
        public class RoomInfo
        {
            public int recordID;
            public int state;
            public string access;
            public int[] taskIDs;
            public string[] users;
            public MesgInfo[] mesgs;
            public string ToStringUserList()
            {
                string ret = "";
                if (users == null)
                    return ret;

                foreach (var item in users)
                {
                    ret += item;
                    ret += ",";
                }
                return ret.Substring(0, ret.Length - 1);
            }
        }

        [Serializable]
        public class MesgInfo
        {
            public int recordID;
            public int tick;
            public long time;
            public string user;
            public string message;
        }
        
        [Serializable]
        public class Work
        {
            public int recordID;
            public int cmdID;
            public int progress;
            public int chatID;
            public int currentState;
            public long time;
            public long launch;
            public long term;
            public long due;
            public long timeFirst;
            public long timeDone;
            public string type;
            public string access;
            public string mainCate;
            public string subCate;
            public string title;
            public string comment;
            public string creator;
            public string director;
            public string worker;
            public string state;
            public string priority;
        }

        public class WorkList : HEADER
        {
            public Work[] works;

            public WorkList(int n = 1)
            {
                works = new Work[n];
                for (int i = 0; i < n; ++i)
                    works[i] = new Work();
            }

            public override byte[] Serialize()
            {
                List<byte> ary = new List<byte>();

                byte[] bodyBuf = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, works);
                    bodyBuf = ms.ToArray();
                }

                HEADER head = Clone();
                head.msgSize = HeaderSize() + bodyBuf.Length;
                ary.AddRange(head.Serialize());
                ary.AddRange(bodyBuf);

                return ary.ToArray();
            }

            public override void Deserialize(ref byte[] data)
            {
                int headSize = HEADER.HeaderSize();
                int bodySize = data.Length - headSize;

                byte[] headBuf = new byte[headSize];
                Array.Copy(data, 0, headBuf, 0, headSize);
                HEADER head = new HEADER();
                head.Deserialize(ref headBuf);

                msgID = head.msgID;
                msgSize = head.msgSize;
                msgSOF = head.msgSOF;
                msgType = head.msgType;
                msgErr = head.msgErr;
                msgUser = head.msgUser;
                msgTime = head.msgTime;
                ext1 = head.ext1;
                ext2 = head.ext2;

                byte[] bodyBuf = new byte[bodySize];
                Array.Copy(data, headSize, bodyBuf, 0, bodySize);
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(bodyBuf))
                {
                    works = (Work[])bf.Deserialize(ms);
                }
            }
        }


        [Serializable]
        public class WorkHistory
        {
            public int recordID;
            public int taskID;
            public long time;
            public string editor;
            public string columnName;
            public string fromInfo;
            public string toInfo;
        }

        public class WorkHistoryList : HEADER
        {
            public WorkHistory[] workHistory;

            public WorkHistoryList(int n = 1)
            {
                workHistory = new WorkHistory[n];
                for (int i = 0; i < n; ++i)
                    workHistory[i] = new WorkHistory();
            }

            public override byte[] Serialize()
            {
                List<byte> ary = new List<byte>();

                byte[] bodyBuf = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, workHistory);
                    bodyBuf = ms.ToArray();
                }

                HEADER head = Clone();
                head.msgSize = HeaderSize() + bodyBuf.Length;
                ary.AddRange(head.Serialize());
                ary.AddRange(bodyBuf);

                return ary.ToArray();
            }

            public override void Deserialize(ref byte[] data)
            {
                int headSize = HEADER.HeaderSize();
                int bodySize = data.Length - headSize;

                byte[] headBuf = new byte[headSize];
                Array.Copy(data, 0, headBuf, 0, headSize);
                HEADER head = new HEADER();
                head.Deserialize(ref headBuf);

                msgID = head.msgID;
                msgSize = head.msgSize;
                msgSOF = head.msgSOF;
                msgType = head.msgType;
                msgErr = head.msgErr;
                msgUser = head.msgUser;
                msgTime = head.msgTime;
                ext1 = head.ext1;
                ext2 = head.ext2;

                byte[] bodyBuf = new byte[bodySize];
                Array.Copy(data, headSize, bodyBuf, 0, bodySize);
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(bodyBuf))
                {
                    workHistory = (WorkHistory[])bf.Deserialize(ms);
                }
            }
        }
    }
}
