using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.Extensions
{
    public static class CompatibilityExtensions
    {
#if NETSTANDARD2_0
        public static bool TryAdd<K, V>(this Dictionary<K, V> dict, K key, V val)
        {
            if(dict.ContainsKey(key))
            {
                return false;
            }

            dict.Add(key, val);

            return true;
        }

        public static bool Remove<K, V>(this Dictionary<K, V> dict, K key, out V val)
        {
            if(dict.TryGetValue(key, out val))
            {
                dict.Remove(key);

                return true;
            }

            val = default;

            return false;
        }
#endif
    }
}
