using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aix.ConfigWrapper.ConsulEx
{
    public class ConsulConfigurationProvider : BaseConfigurationProvider
    {
        ConsulConfigurationOption _option;

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
                // Console.WriteLine("Consul配置加载成功");

                ConvertToJsonConfiguration();
            }
            catch (Exception ex)
            {
                throw new Exception("Consul配置加载异常", ex);
            }
        }

        private void AddByPrefix(ConsulClient consulClient, string prefix)
        {
            var list = consulClient.KV.List(prefix).Result;
            foreach (var item in list.Response)
            {
                string pathKey = item.Key;
                if (!string.IsNullOrEmpty(pathKey) && item.Value != null && item.Value.Length > 0)
                {
                    var strValue = Encoding.UTF8.GetString(item.Value);
                    base.AddData(GetKeyName(pathKey), strValue);
                }
            }
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
}
