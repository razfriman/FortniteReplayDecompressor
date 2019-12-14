using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core
{
    public class FBitArray
    {
        private bool[] _items;

        public int Length => _items.Length;
        public bool IsReadOnly => false;

        public FBitArray(bool[] bits)
        {
            _items = bits;
        }

        public unsafe FBitArray(byte[] bytes)
        {
            _items = new bool[bytes.Length * 8];

            fixed (byte* bytePtr = bytes)
            fixed (bool* itemPtr = _items)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    int offset = i * 8;

                    byte b = *(bytePtr + i);

                    for (int z = 0; z < 8; z++)
                    {
                        *(itemPtr + offset + z) = (b & 0x01) == 0x01;

                        b >>= 1;
                    }
                }
            }
        }

        public bool this[int index]
        { 
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        public Span<bool> AsSpan(int start, int count)
        {
            return _items.AsSpan(start, count);
        }
        public void CopyTo(bool[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public void Append(bool[] after)
        {
            bool[] newArray = new bool[_items.Length + after.Length];

            Array.Copy(_items, 0, newArray, 0, _items.Length);
            Array.Copy(after, 0, newArray, _items.Length, after.Length);

            _items = newArray;
        }
    }
}
