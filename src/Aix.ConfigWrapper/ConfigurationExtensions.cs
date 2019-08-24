using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.ConfigWrapper
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddFileConfiguration(
          this IConfigurationBuilder builder, FileConfigurationOption  option)
        {
            return builder.Add(new FileConfigurationSource(option));
        }
        
    }
}
