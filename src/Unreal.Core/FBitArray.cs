using OozSharp.MemoryPool;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unreal.Core.Models;

namespace Unreal.Core
{
    public unsafe sealed class FBitArray : IDisposable
    {
        private bool* _pointer;

        public ReadOnlyMemory<bool> Items { get; private set; }
        private IPinnedMemoryOwner<bool> _owner;

        public int Length { get; private set; }
        public bool IsReadOnly => false;

        public FBitArray()
        {

        }

        public FBitArray(byte* ptr, int byteCount, int totalBits)
        {
            _owner = PinnedMemoryPool<bool>.Shared.Rent(totalBits);
            Items = _owner.PinnedMemory.Memory;
            Length = totalBits;
            _pointer = (bool*)_owner.PinnedMemory.Pointer;

            for (int i = 0; i < byteCount; i++)
            {
                int offset = i * 8;
                byte deref = *(ptr + i);

                *(_pointer + offset) = (deref & 0x01) == 0x01;
                *(_pointer + offset + 1) = (deref & 0x02) == 0x02;
                *(_pointer + offset + 2) = (deref & 0x04) == 0x04;
                *(_pointer + offset + 3) = (deref & 0x08) == 0x08;
                *(_pointer + offset + 4) = (deref & 0x10) == 0x10;
                *(_pointer + offset + 5) = (deref & 0x20) == 0x20;
                *(_pointer + offset + 6) = (deref & 0x40) == 0x40;
                *(_pointer + offset + 7) = (deref & 0x80) == 0x80;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetAsByte(int index)
        {
            return (*(byte*)(_pointer + index));
        }

        public void Append(ReadOnlyMemory<bool> after)
        {
            IPinnedMemoryOwner<bool> newOwner = PinnedMemoryPool<bool>.Shared.Rent(after.Length + Length);
            Memory<bool> newMemory = newOwner.PinnedMemory.Memory;
            int oldLength = Length;
            Length = after.Length + Length;

            //Copy old array
            Items.CopyTo(newMemory);
            //Buffer.MemoryCopy(_pointer, newOwner.PinnedMemory.Pointer, newOwner.PinnedMemory.Length, Length);

            Items = newMemory;

            Unpin(); //Get rid of old

            _owner = newOwner; 
            _pointer = (bool*)_owner.PinnedMemory.Pointer;

            MemoryHandle afterPin = after.Pin();

            Buffer.MemoryCopy(afterPin.Pointer, _pointer + oldLength, after.Length, after.Length);

            afterPin.Dispose();
        }

        public void Dispose()
        {
            Unpin();
        }

        private void Unpin()
        {
            _owner?.Dispose();
            _owner = null;
            _pointer = null;
        }
    }
}
