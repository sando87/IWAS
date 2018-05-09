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
            public uint sof;
            [MarshalAs(UnmanagedType.U4)]
            public uint id;
            [MarshalAs(UnmanagedType.U4)]
            public uint size;
            [MarshalAs(UnmanagedType.U4)]
            public uint type;
            [MarshalAs(UnmanagedType.U4)]
            public uint error;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public string time;

            static public void FillHeader(object obj, COMMAND id, TYPE type)
            {
                HEADER head = obj as HEADER;
                head.id = (uint)id;
                head.size = (uint)Marshal.SizeOf(obj);
                head.sof = (uint)MAGIC.SOF;
                head.type = (uint)type;
                head.error = (uint)ERRORCODE.NOERROR;
                head.time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            }
            static public int HeaderSize()
            {
                return Marshal.SizeOf(typeof(HEADER));
            }
            static public HEADER GetHeaderInfo(byte[] buf)
            {
                HEADER obj = new HEADER();
                int headSize = HeaderSize();
                byte[] headBuf = new byte[headSize];
                Array.Copy(buf, headBuf, headSize);
                Deserialize(obj, ref headBuf);
                return obj;
            }

            static public byte[] Serialize(object obj)
            {
                // allocate a byte array for the struct data
                var buffer = new byte[Marshal.SizeOf(obj)];

                // Allocate a GCHandle and get the array pointer
                var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                var pBuffer = gch.AddrOfPinnedObject();

                // copy data from struct to array and unpin the gc pointer
                Marshal.StructureToPtr(obj, pBuffer, false);
                gch.Free();

                return buffer;
            }

            static public void Deserialize(Object obj, ref byte[] data)
            {
                var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                Marshal.PtrToStructure(gch.AddrOfPinnedObject(), obj);
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string kind;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string access;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string mainCategory;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string subCategory;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string title;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string creator;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string director;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string worker;
            [MarshalAs(UnmanagedType.U4)]
            public uint preLaunch;
            [MarshalAs(UnmanagedType.U4)]
            public uint preterm;
            [MarshalAs(UnmanagedType.U4)]
            public uint preDue;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string state;
            [MarshalAs(UnmanagedType.U4)]
            public uint progress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string priority;

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
            public string user;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string access;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string fileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string fileDir;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
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
