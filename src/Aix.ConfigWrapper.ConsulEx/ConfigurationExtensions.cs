using Microsoft.Extensions.Configuration;
using System;

namespace Aix.ConfigWrapper.ConsulEx
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddConsulConfiguration(
         this IConfigurationBuilder builder, ConsulConfigurationOption option)
        {
            return builder.Add(new ConsulConfigurationSource(option));
        }

    }
}
