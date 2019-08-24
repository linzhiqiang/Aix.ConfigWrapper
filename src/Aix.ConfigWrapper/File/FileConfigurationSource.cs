using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.ConfigWrapper
{
    public class FileConfigurationSource : IConfigurationSource
    {
        FileConfigurationOption _option;
        public FileConfigurationSource(FileConfigurationOption option)
        {
            _option = option;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new FileConfigurationProvider(_option);
        }
    }
}
