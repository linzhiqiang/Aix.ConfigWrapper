using Microsoft.Extensions.Configuration;
using System;

namespace Aix.ConfigWrapper.MySqlEx
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddDBMysqlConfiguration(
            this IConfigurationBuilder builder, DBConfigurationOption option)
        {
            return builder.Add(new DBConfigurationSource(option));
        }

    }
}
