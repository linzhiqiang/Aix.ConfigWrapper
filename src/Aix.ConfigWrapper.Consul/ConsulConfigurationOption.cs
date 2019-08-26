using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.ConfigWrapper.Consul
{
    public class ConsulConfigurationOption
    {
        public string Url { get; set; }

        public string[] Prefixs { get; set; }
    }
}
