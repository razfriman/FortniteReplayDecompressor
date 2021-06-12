using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OozSharp.MemoryPool
{
    internal sealed partial class PinnedArrayMemoryPool<T> : PinnedMemoryPool<T>
    {
        private const int MaximumBufferSize = int.MaxValue;

        public sealed override int MaxBufferSize => MaximumBufferSize;

        public sealed override IPinnedMemoryOwner<T> Rent(int minimumBufferSize = -1)
        {
            return new ArrayMemoryPoolBuffer(minimumBufferSize);
        }

        protected sealed override void Dispose(bool disposing) { } 
    }

    internal sealed partial class PinnedArrayMemoryPool<T> : PinnedMemoryPool<T>
    {
        private sealed class ArrayMemoryPoolBuffer : IPinnedMemoryOwner<T>
        {
            private PinnedMemory<T> _array;
            private static PinnedArrayPool<T> _pool = new PinnedArrayPool<T>();

            public PinnedMemory<T> PinnedMemory
            {
                get
                {
                    return _array;
                }
            }

            public ArrayMemoryPoolBuffer(int size)
            {
                _array = _pool.Rent(size);
            }

            public void Dispose()
            {
                PinnedMemory<T> array = _array;

                if (array != null)
                {
                    _array = null;
                    _pool.Return(array);
                }
            }
        }
    }
}
