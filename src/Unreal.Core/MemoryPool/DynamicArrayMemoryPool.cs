using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.MemoryPool
{
    internal sealed partial class DynamicArrayMemoryPool<T> : PinnedMemoryPool<T>
    {
        private const int MaximumBufferSize = int.MaxValue;

        public sealed override int MaxBufferSize => MaximumBufferSize;

        public sealed override IMemoryOwner<T> Rent(int minimumBufferSize = -1)
        {
            return new ArrayMemoryPoolBuffer(minimumBufferSize);
        }

        protected sealed override void Dispose(bool disposing) { } 
    }

    internal sealed partial class DynamicArrayMemoryPool<T> : PinnedMemoryPool<T>
    {
        private sealed class ArrayMemoryPoolBuffer : IMemoryOwner<T>
        {
            private T[]? _array;
            private static ArrayPool<T> _pool = new TlsOverPerCoreLockedStacksArrayPool<T>();

            public ArrayMemoryPoolBuffer(int size)
            {
                _array = _pool.Rent(size);
            }

            public Memory<T> Memory
            {
                get
                {
                    T[]? array = _array;

                    if (array == null)
                    {

                    }

                    return new Memory<T>(array);
                }
            }

            public void Dispose()
            {
                T[]? array = _array;
                if (array != null)
                {
                    _array = null;
                    _pool.Return(array);
                }
            }
        }
    }
}
