using OozSharp.MemoryPool;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace OozSharp
{
    public unsafe class KrakenDecoder : IDisposable
    {
        internal int SourceUsed { get; set; }
        internal int DestinationUsed { get; set; }
        internal KrackenHeader Header { get; set; }
        internal int ScratchSize { get; set; } = 0x6C000;
        internal byte* Scratch { get; set; }

        private IPinnedMemoryOwner<byte> _scratchOwner;

        public KrakenDecoder()
        {
            _scratchOwner = PinnedMemoryPool<byte>.Shared.Rent(ScratchSize);
            Scratch = (byte*)_scratchOwner.PinnedMemory.Pointer;

        }
        public void Dispose()
        {
            _scratchOwner?.Dispose();
        }
    }
}
