using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        private IMemoryOwner<bool> _owner;

        public int Length { get; private set; }
        public bool IsReadOnly => false;
        public byte[] ByteArrayUsed;

        public FBitArray()
        {

        }

        /*
        public FBitArray(ReadOnlyMemory<bool> bits)
        {
            Items = bits;
            Length = bits.Length;
            //_items = Items;
        }*/

        public FBitArray(byte[] bytes)
        {
            ByteArrayUsed = bytes;
            int totalBits = bytes.Length * 8;

            _owner = MemoryPool<bool>.Shared.Rent(totalBits);
            Items = _owner.Memory;
            Length = totalBits;
            Pin();


            fixed (byte* bytePtr = bytes)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    int offset = i * 8;

                    *(_pointer + offset) = ((*(bytePtr + i)) & 0x01) == 0x01;
                    *(_pointer + offset + 1) = ((*(bytePtr + i) >> 1) & 0x01) == 0x01;
                    *(_pointer + offset + 2) = ((*(bytePtr + i) >> 2) & 0x01) == 0x01;
                    *(_pointer + offset + 3) = ((*(bytePtr + i) >> 3) & 0x01) == 0x01;
                    *(_pointer + offset + 4) = ((*(bytePtr + i) >> 4) & 0x01) == 0x01;
                    *(_pointer + offset + 5) = ((*(bytePtr + i) >> 5) & 0x01) == 0x01;
                    *(_pointer + offset + 6) = ((*(bytePtr + i) >> 6) & 0x01) == 0x01;
                    *(_pointer + offset + 7) = ((*(bytePtr + i) >> 7) & 0x01) == 0x01;
                }
            }

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

        public void Append(ReadOnlyMemory<bool> after)
        {
            IMemoryOwner<bool> newOwner = MemoryPool<bool>.Shared.Rent(after.Length + Length);
            Memory<bool> newMemory = newOwner.Memory;

            int oldLength = Length;
            Length = after.Length + Length;

            //Copy old array
            Items.CopyTo(newMemory);
            Items = newMemory;

            Unpin(); //Get rid of old
            Pin(); //Pin new

            MemoryHandle afterPin = after.Pin();
            bool* afterPointer = (bool*)afterPin.Pointer;

            for(int i = 0; i < after.Length; i++)
            {
                *(_pointer + oldLength + i) = *(afterPointer + i);
            }

            afterPin.Dispose();

            _owner = newOwner;
        }

        public void Dispose()
        {
            Unpin();
        }

        private void Pin()
        {
            _pin = Items.Pin();
            _pointer = (bool*)_pin.Pointer;
        }

        private void Unpin()
        {
            if (_owner != null)
            {
                _owner.Dispose();
                _owner = null;
                _pin.Dispose();
                _pin = new MemoryHandle();
                _pointer = null;
            }
        }
    }
}
