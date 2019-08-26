using Microsoft.Extensions.Configuration;
using System;

namespace Aix.ConfigWrapper.DB
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddDBConfiguration(
            this IConfigurationBuilder builder, DBConfigurationOption option)
        {
            return builder.Add(new DBConfigurationSource(option));
        }

    }
}
