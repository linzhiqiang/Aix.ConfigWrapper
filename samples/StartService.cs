using Aix.ConfigWrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    public class StartService : IHostedService
    {
        private IConfiguration _configuration;
        public StartService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
           var hps4 = _configuration.GetSection("hps").Get<HpsCfg>();

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/appsettings.json");
            using (var fs = File.OpenRead(path))
            {
                var data = JsonConfigurationFileParser.Parse(fs);
            }


            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
