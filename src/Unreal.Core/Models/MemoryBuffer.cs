using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.Models
{
    public unsafe sealed class MemoryBuffer : IDisposable
    {
        public UnmanagedMemoryStream Stream { get; private set; }
        public int Size { get; private set; }
        public byte* PositionPointer { get; private set; }
        public Memory<byte> Memory { get; private set; }

        private IMemoryOwner<byte> _owner;
        private MemoryHandle _pin;

        public MemoryBuffer(int bytes)
        {
            //Slightly faster
            if (bytes <= 1000)
            {
                Memory = new Memory<byte>(new byte[bytes]);
            }
            else
            {
                _owner = MemoryPool<byte>.Shared.Rent(bytes);
                Memory = _owner.Memory;
            }

            _pin = Memory.Pin();
            PositionPointer = (byte*)_pin.Pointer;
            Size = bytes;
            Stream = new UnmanagedMemoryStream(PositionPointer, bytes, bytes, FileAccess.ReadWrite);
        }

        public void Dispose()
        {
            _owner?.Dispose();
            _pin.Dispose();
            Stream?.Dispose();
        }
    }
}
