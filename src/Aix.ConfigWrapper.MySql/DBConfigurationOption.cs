using System;
using System.Collections.Generic;
using System.Text;

namespace Aix.ConfigWrapper.DB
{
 public   class DBConfigurationOption
    {
        public string ConfigConnectionString { get; set; }

        public string AppCode { get; set; }

        public string[] Groups { get; set; }


    }
}
