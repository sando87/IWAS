using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWAS
{
    class FifoBuffer
    {
        private List<byte> mi_FifoData = new List<byte>();

        /// <summary>
        /// Get the count of bytes in the Fifo buffer
        /// </summary>
        public int Count
        {
            get
            {
                lock (mi_FifoData)
                {
                    return mi_FifoData.Count;
                }
            }
        }

        /// <summary>
        /// Clears the Fifo buffer
        /// </summary>
        public void Clear()
        {
            lock (mi_FifoData)
            {
                mi_FifoData.Clear();
            }
        }

        /// <summary>
        /// Append data to the end of the fifo
        /// </summary>
        public void Push(byte[] buf)
        {
            lock (mi_FifoData)
            {
                // Internally the .NET framework uses Array.Copy() which is extremely fast
                mi_FifoData.AddRange(buf);
            }
        }

        /// <summary>
        /// Get data from the beginning of the fifo.
        /// returns null if s32_Count bytes are not yet available.
        /// </summary>
        public byte[] Pop(int size, bool isKeep = false)
        {
            lock (mi_FifoData)
            {
                if (mi_FifoData.Count < size)
                    LOG.warn();

                // Internally the .NET framework uses Array.Copy() which is extremely fast
                byte[] dest = new byte[size];
                mi_FifoData.CopyTo(0, dest, 0, size);
                if(!isKeep)
                {
                    mi_FifoData.RemoveRange(0, size);
                }
                
                return dest;
            }
        }

        /// <summary>
        /// Gets a byte without removing it from the Fifo buffer
        /// returns -1 if the index is invalid
        /// </summary>
        public int PeekAt(int idx)
        {
            lock (mi_FifoData)
            {
                if (idx < 0 || idx >= mi_FifoData.Count)
                    return -1;

                return mi_FifoData[idx];
            }
        }

        public byte this[int index]
        {
            get
            {
                lock (mi_FifoData)
                {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException();

                    return mi_FifoData[index];
                }
            }
        }

        public short readShort(int index)
        {
            lock (mi_FifoData)
            {
                int idxA = index * 2;
                int idxB = index * 2 + 1;
                if (index < 0 || idxB >= Count)
                    throw new ArgumentOutOfRangeException();

                ushort a = mi_FifoData[idxA];
                ushort b = mi_FifoData[idxB];
                return (short)(a | (b << 8));
            }
        }

        public int readInt(int index)
        {
            lock (mi_FifoData)
            {
                int idxA = index * 2;
                int idxB = index * 2 + 1;
                int idxC = index * 2 + 2;
                int idxD = index * 2 + 3;
                if (index < 0 || idxD >= Count)
                    throw new ArgumentOutOfRangeException();

                uint a = mi_FifoData[idxA];
                uint b = mi_FifoData[idxB];
                uint c = mi_FifoData[idxC];
                uint d = mi_FifoData[idxD];
                return (int)(a | (b<<8) | (c<<16) | (d<<24));
            }
        }
    }
}
