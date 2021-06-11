using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.Extensions
{
    public class KeyList<K, V>
    {
        private List<V> _vals = new List<V>();
        private Dictionary<K, int> _keys = new Dictionary<K, int>();

        public void Add(K key, V val)
        {
            if(_keys.TryAdd(key, _vals.Count))
            {
                _vals.Add(val);
            }
        }

        public bool TryGetIndex(K key, out int index)
        {
            return _keys.TryGetValue(key, out index);
        }

        public bool TryGetValue(int keyId, out V val)
        {
            val = default;

            if(keyId >= 0 && keyId < _vals.Count)
            {
                val = _vals[keyId];

                return true;
            }

            return false;
        }

        public bool TryGetValue(K key, out V val)
        {
            if(_keys.TryGetValue(key, out int id))
            {
                val = _vals[id];

                return true;
            }

            val = default;

            return false;
        }

        public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<K, TKey> key, Func<V, TElement> val)
        {
            Dictionary<TKey, TElement> vals = new Dictionary<TKey, TElement>();

            foreach(var kvp in _keys)
            {
                V itemVal = _vals[kvp.Value];

                vals.Add(key(kvp.Key), val(itemVal));
            }

            return vals;
        }

        public V this[int index]
        {
            get
            {
                return _vals[index];
            }
        }
    }
}
