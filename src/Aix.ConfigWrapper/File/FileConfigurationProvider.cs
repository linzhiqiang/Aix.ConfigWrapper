using Microsoft.Extensions.Configuration;
using System;

namespace Aix.ConfigWrapper
{
    public class FileConfigurationProvider : BaseConfigurationProvider
    {
        FileConfigurationOption _option;
        public FileConfigurationProvider(FileConfigurationOption option)
        {
            _option = option;
        }
        public override void Load()
        {
            if (_option.ConfigFiles == null) return;
            try
            {
                var kvs = ConfigFileParserTools.ParseKV(_option.ConfigFiles);
                foreach (var item in kvs)
                {
                    base.AddData(item.Key, item.Value);
                }
                //  Console.WriteLine("File配置加载成功");
            }
            catch (Exception ex)
            {
                throw new Exception("File配置加载异常", ex);
            }
        }
    }
}
