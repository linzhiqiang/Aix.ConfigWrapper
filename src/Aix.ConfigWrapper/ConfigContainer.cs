using Aix.ConfigWrapper.Utils;
using System;
using System.Collections.Concurrent;

namespace Aix.ConfigWrapper
{
    public class ConfigContainer
    {
        public static ConfigContainer Instance = new ConfigContainer();

        public event Action<ConfigChangeInfo> OnConfigChange;

        public void Change(ConfigChangeInfo changeInfo)
        {
            if (OnConfigChange != null)
            {
                OnConfigChange(changeInfo);
            }
        }

    }

    public class ConfigChangeInfo
    {
        public string GroupCode { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }

}


