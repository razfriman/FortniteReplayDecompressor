using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core
{
    public class FBitArray
    {
        public bool[] Items { get; private set; }

        public int Length => Items.Length;
        public bool IsReadOnly => false;
        public byte[] ByteArrayUsed;

        public FBitArray(bool[] bits)
        {
            Items = bits;
        }

        public unsafe FBitArray(byte[] bytes)
        {
            Items = new bool[bytes.Length * 8];
            ByteArrayUsed = bytes;

            fixed (byte* bytePtr = bytes)
            fixed (bool* itemPtr = Items)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    int offset = i * 8;

                    *(itemPtr + offset) = ((*(bytePtr + i)) & 0x01) == 0x01;
                    *(itemPtr + offset + 1) = ((*(bytePtr + i) >> 1) & 0x01) == 0x01;
                    *(itemPtr + offset + 2) = ((*(bytePtr + i) >> 2) & 0x01) == 0x01;
                    *(itemPtr + offset + 3) = ((*(bytePtr + i) >> 3) & 0x01) == 0x01;
                    *(itemPtr + offset + 4) = ((*(bytePtr + i) >> 4) & 0x01) == 0x01;
                    *(itemPtr + offset + 5) = ((*(bytePtr + i) >> 5) & 0x01) == 0x01;
                    *(itemPtr + offset + 6) = ((*(bytePtr + i) >> 6) & 0x01) == 0x01;
                    *(itemPtr + offset + 7) = ((*(bytePtr + i) >> 7) & 0x01) == 0x01;
                }
            }
        }

        public bool this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = value;
            }
        }

#if !NETSTANDARD2_0
        public Span<bool> AsSpan(int start, int count)
        {
            return Items.AsSpan(start, count);
        }
#endif

        public void CopyTo(bool[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public void Append(bool[] after)
        {
            bool[] newArray = new bool[Items.Length + after.Length];

            Array.Copy(Items, 0, newArray, 0, Items.Length);
            Array.Copy(after, 0, newArray, Items.Length, after.Length);

            Items = newArray;
        }
    }
}
