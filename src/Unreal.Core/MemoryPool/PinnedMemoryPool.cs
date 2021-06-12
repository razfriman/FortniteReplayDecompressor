using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.MemoryPool
{
    public abstract class PinnedMemoryPool<T> : IDisposable
    {
        // Store the shared ArrayMemoryPool in a field of its derived sealed type so the Jit can "see" the exact type
        // when the Shared property is inlined which will allow it to devirtualize calls made on it.
        private static readonly DynamicArrayMemoryPool<T> s_shared = new DynamicArrayMemoryPool<T>();

        /// <summary>
        /// Returns a singleton instance of a MemoryPool based on arrays.
        /// </summary>
        public static PinnedMemoryPool<T> Shared => s_shared;

        /// <summary>
        /// Returns a memory block capable of holding at least <paramref name="minBufferSize" /> elements of T.
        /// </summary>
        /// <param name="minBufferSize">If -1 is passed, this is set to a default value for the pool.</param>
        public abstract IMemoryOwner<T> Rent(int minBufferSize = -1);

        /// <summary>
        /// Returns the maximum buffer size supported by this pool.
        /// </summary>
        public abstract int MaxBufferSize { get; }

        /// <summary>
        /// Constructs a new instance of a memory pool.
        /// </summary>
        protected PinnedMemoryPool() { }

        /// <summary>
        /// Frees all resources used by the memory pool.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees all resources used by the memory pool.
        /// </summary>
        /// <param name="disposing"></param>
        protected abstract void Dispose(bool disposing);
    }
}
