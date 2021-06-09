using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unreal.Core
{
    public unsafe class FBitArray : IDisposable
    {
        public static int Pins;

        private MemoryHandle _pin;
        private bool* _pointer;

        public ReadOnlyMemory<bool> Items { get; private set; }
        private IMemoryOwner<bool> owner;

        public int Length { get; private set; }
        public bool IsReadOnly => false;
        public byte[] ByteArrayUsed;

        public FBitArray()
        {

        }

        public FBitArray(ReadOnlyMemory<bool> bits)
        {
            Items = bits;
            Length = bits.Length;
            //_items = Items;
        }

        public FBitArray(byte[] bytes)
        {
            int totalBits = bytes.Length * 8;
            Length = totalBits;

            Pin();

            bool[] tArray = new bool[bytes.Length * 8];
            ByteArrayUsed = bytes;

            fixed (byte* bytePtr = bytes)
            fixed (bool* itemPtr = tArray)
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

            Items = tArray;
            Length = Items.Length;
            Pin();
        }

        public FBitArray Slice(int start, int count)
        {
            FBitArray fBitArray = new FBitArray();
            fBitArray.Items = Items.Slice(start, count);
            fBitArray._pointer = _pointer + start;
            fBitArray.Length = count;

            return fBitArray;
        }

        public bool this[int index]
        {
            get
            {
                return *(_pointer + index);
            }
        }

#if !NETSTANDARD2_0
        public ReadOnlyMemory<bool> AsSpan(int start, int count)
        {
            return Items.Slice(start, count);
        }
#endif

        public void Append(ReadOnlyMemory<bool> after)
        {
            Unpin();

            bool[] newArray = new bool[Items.Length + after.Length];

            Array.Copy(after.ToArray(), 0, newArray, Items.Length, after.Length);

            Memory<bool> mArray = new Memory<bool>(newArray);

            Items.CopyTo(mArray);
            Items = mArray;

            Pin();
        }

        public void Dispose()
        {
            Unpin();
        }

        private void Pin()
        {
            _pin = Items.Pin();
            _pointer = (bool*)_pin.Pointer;
            Interlocked.Increment(ref Pins);
        }

        private void Unpin()
        {
            if (_pin.Pointer != new MemoryHandle().Pointer)
            {
                _pin.Dispose();
                _pin = new MemoryHandle();
                _pointer = null;
                Interlocked.Decrement(ref Pins);
            }
        }
    }
}
