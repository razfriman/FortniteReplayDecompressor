using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OozSharp.MemoryPool
{
    public unsafe interface IPinnedMemoryOwner<T> : IDisposable
    {
        public PinnedMemory<T> PinnedMemory { get; }
    }
}
