using Aix.ConfigWrapper.Utils;
using System;
using System.Collections.Concurrent;

namespace Aix.ConfigWrapper
{
    public class ConfigContainer
    {
        public static ConfigContainer Instance = new ConfigContainer();

        ConcurrentDictionary<string, string> Data = new ConcurrentDictionary<string, string>();

        public void Set(string key, string value)
        {
            if (Data.ContainsKey(key))
            {
                Data[key] = value;
            }
            else
            {
                Data.TryAdd(key, value);
            }
        }

        public string GetString(string key)
        {
            if (Data.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }

        public T Get<T>(string key)
        {
            var strValue = GetString(key);
            if (string.IsNullOrEmpty(strValue))
            {
                return default(T);
            }
            if (typeof(T) == typeof(string) || typeof(T).IsValueType)
            {
                return (T)Convert.ChangeType(strValue, typeof(T));
            }

            return JsonUtils.FromJson<T>(strValue);
        }

    }

}


