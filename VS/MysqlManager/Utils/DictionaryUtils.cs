using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysqlManager.Entity;

namespace MysqlManager.Utils
{
    public class DictionaryUtils
    {
        public static T2 getValue<T1,T2>(Dictionary<T1, T2> dict, T1 key) {
            T2 value;
            bool success = dict.TryGetValue(key, out value);
            if (success)
            {
                return value;
            }
            else {
                return default(T2);
            }
        }

        public static SerializableDictionary<TKey, TValue> update<TKey, TValue>(SerializableDictionary<TKey, TValue> dict, TKey key, TValue newValue)
        {
            dict.Remove(key);
            dict.Add(key, newValue);
            return dict;
        }
    }
}
