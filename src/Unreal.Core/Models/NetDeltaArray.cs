using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Models
{
    public class NetDeltaArray<T>
    {
        public ICollection<T> Items => _items.Values;

        private Dictionary<int, T> _items = new Dictionary<int, T>();

        public bool DeleteIndex(int index)
        {
            return _items.Remove(index);
        }

        public bool AddItem(int index, T item)
        {
            return _items.TryAdd(index, item);
        }

        public bool TryGetItem(int index, out T item)
        {
            return _items.TryGetValue(index, out item);
        }
    }
}
