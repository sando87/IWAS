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
        public enum COMMAND
        {
            NONE,
            NewUser,
            UserList,
            Login,
            Logout,
            TaskNew,
            TaskEdit,
            TaskDelete,
            TaskList,
            TaskInfo,
            NewChat,
            AddChat,
            UploadFile,
            DownloadFile,
            LogMessage,
            Search,
            MAX_COUNT,
        }
        public enum TYPE
        {
            REQ,
            ACK,
            REP,
            ALT,
        }
        public enum MAGIC
        {
            SOF = 0xaa,
            EOF = 0xbb
        }
        public enum ERRORCODE
        {
            NOERROR,
            HaveID,
            NoID,
            WorngPW,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class HEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint msgSOF;
            [MarshalAs(UnmanagedType.U4)]
            public uint msgID;
            [MarshalAs(UnmanagedType.U4)]
            public uint msgSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint msgType;
            [MarshalAs(UnmanagedType.U4)]
            public uint msgErr;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string msgUser;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string msgTime;

            public void FillClientHeader(COMMAND id)
            {
                msgID = (uint)id;
                msgSize = (uint)Marshal.SizeOf(this);
                msgSOF = (uint)MAGIC.SOF;
                msgType = (uint)TYPE.REQ;
                msgErr = (uint)ERRORCODE.NOERROR;
                msgUser = MyInfo.mMyInfo.msgUser;
                msgTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            }
            public void FillHeader(COMMAND id, TYPE type, string user)
            {
                msgID = (uint)id;
                msgSize = (uint)Marshal.SizeOf(this);
                msgSOF = (uint)MAGIC.SOF;
                msgType = (uint)type;
                msgErr = (uint)ERRORCODE.NOERROR;
                msgUser = user;
                msgTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            }
            public void FillServerHeader(COMMAND id)
            {
                msgID = (uint)id;
                msgSize = (uint)Marshal.SizeOf(this);
                msgSOF = (uint)MAGIC.SOF;
                msgType = (uint)TYPE.REP;
                msgErr = (uint)ERRORCODE.NOERROR;
                msgUser = ConstDefines.SYSNAME;
                msgTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
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
            [MarshalAs(UnmanagedType.U4)]
            public uint taskID;
            [MarshalAs(UnmanagedType.U4)]
            public uint cmdID;
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
            [MarshalAs(UnmanagedType.U4)]
            public uint progress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string priority;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class TaskEdit : HEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint editTaskID;
            [MarshalAs(UnmanagedType.U4)]
            public uint taskID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string info;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Chat : HEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint roomID;
            [MarshalAs(UnmanagedType.U4)]
            public uint cmdID;
            [MarshalAs(UnmanagedType.U4)]
            public uint taskID;
            [MarshalAs(UnmanagedType.U4)]
            public uint FileID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string createTime;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string user;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string access;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string priority;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string message;

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class File : HEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint fileID;
            [MarshalAs(UnmanagedType.U4)]
            public uint filsSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint chatID;
            [MarshalAs(UnmanagedType.U4)]
            public uint packetIDX;
            [MarshalAs(UnmanagedType.U4)]
            public uint packetCNT;
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024*4)]
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
