using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.ConfigWrapper
{
 public   class FileConfigurationOption
    {
        public IConfigurationBuilder ConfigurationBuilder { get; set; }
        public string[] ConfigFiles { get; set; }
    }
}
