using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.ConfigWrapper.Consul
{
    public class ConsulConfigurationSource : IConfigurationSource
    {
        ConsulConfigurationOption _option;
        public ConsulConfigurationSource(ConsulConfigurationOption option)
        {
            _option = option;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(_option);
        }
    }
}
