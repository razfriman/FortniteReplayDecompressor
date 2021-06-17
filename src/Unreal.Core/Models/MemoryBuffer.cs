using OozSharp.MemoryPool;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unreal.Core.Models
{
    public unsafe sealed class MemoryBuffer : IDisposable
    {
        public UnmanagedMemoryStream Stream { get; private set; }
        public int Size { get; private set; }
        public byte* PositionPointer { get; private set; }
        public Memory<byte> Memory { get; private set; }

        private IPinnedMemoryOwner<byte> _pinnedOwner;

        public MemoryBuffer(byte* pointer, int length)
        {
            PositionPointer = pointer;
            //Stream = new UnmanagedMemoryStream(pointer, length, length, FileAccess.ReadWrite);
            Size = length;
        }

        public MemoryBuffer(int bytes)
        {
            _pinnedOwner = PinnedMemoryPool<byte>.Shared.Rent(bytes);
            Memory = _pinnedOwner.PinnedMemory.Memory;

            PositionPointer = (byte*)_pinnedOwner.PinnedMemory.Pointer;
            Size = bytes;
            Stream = new UnmanagedMemoryStream(PositionPointer, bytes, bytes, FileAccess.ReadWrite);
        }

        public void Dispose()
        {
            _pinnedOwner?.Dispose();
            Stream?.Dispose();
        }
    }
}
