using System.Collections.Generic;

namespace UnitSense.Extensions.Extensions
{
    public static class DictionnaryExt
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            TValue ret;
            dict.TryGetValue(key, out ret);
            return ret;
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }
    }
}
