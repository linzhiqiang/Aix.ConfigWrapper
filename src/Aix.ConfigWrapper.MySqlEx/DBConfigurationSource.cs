using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.ConfigWrapper.MySqlEx
{
    public class DBConfigurationSource : IConfigurationSource
    {
        DBConfigurationOption _option;
        public DBConfigurationSource(DBConfigurationOption option)
        {
            _option = option;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DBConfigurationProvider(_option);
        }
    }
}
