using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aix.ConfigWrapper.Consul
{
    public class ConsulConfigurationProvider : BaseConfigurationProvider
    {
        ConsulConfigurationOption _option;
        IDictionary<string, List<ConsulConfigInfo>> ConfigData = new Dictionary<string, List<ConsulConfigInfo>>();
        public ConsulConfigurationProvider(ConsulConfigurationOption option)
        {
            _option = option;
        }
        public override void Load()
        {
            try
            {
                var url = _option.Url;
                if (_option.Prefixs != null && _option.Prefixs.Length > 0)
                {
                    using (var consul = new ConsulClient(c => { c.Address = new Uri(url); }))
                    {
                        foreach (var item in _option.Prefixs)
                        {
                            AddByPrefix(consul, item);
                        }
                    }
                }

                ToJsonConfiguration();
            }
            catch (Exception ex)
            {
                throw new Exception("Consul配置加载异常", ex);
            }
        }

        private void ToJsonConfiguration()
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            foreach (var group in ConfigData)
            {
                foreach (var item in group.Value)
                {
                    AddData(data, item.Key, item.Value);
                }
            }

            base.ConvertToJsonConfiguration(data);
        }

        private void AddByPrefix(ConsulClient consulClient, string prefix)
        {
            var list = consulClient.KV.List(prefix).Result;
            var values = new List<ConsulConfigInfo>();

            foreach (var item in list.Response)
            {
                string pathKey = item.Key;
                if (!string.IsNullOrEmpty(pathKey) && item.Value != null && item.Value.Length > 0)
                {
                    var strValue = Encoding.UTF8.GetString(item.Value);

                    values.Add(new ConsulConfigInfo { group_code = prefix, Key = GetKeyName(pathKey), Value = strValue });
                }
            }

            ConfigData.Add(prefix, values);
        }

        static string GetKeyName(string pathKey)
        {
            var str = pathKey.Trim('/');
            var index = str.LastIndexOf('/');
            if (index >= 0)
            {
                return str.Substring(str.LastIndexOf('/') + 1);
            }
            return str;
        }
    }

    public class ConsulConfigInfo
    {
        public string group_code { get; set; }
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
