using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.Extensions
{
    public sealed class FastClearArray<T>
    {
        public int Count { get; private set; }
        private T[] _array;

        public FastClearArray(int maxCapacity)
        {
            _array = new T[maxCapacity];
        }

        public void Add(T item)
        {
            _array[Count] = item;

            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }


        public T this[int index]
        {
            get
            {
                return _array[index];
            }
            set
            {
                _array[index] = value;
            }
        }
    }
}
