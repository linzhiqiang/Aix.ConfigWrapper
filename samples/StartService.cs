using Aix.ConfigWrapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    public class StartService : IHostedService
    {
        private IConfigService _configService;
        public StartService(IConfigService configService)
        {
            _configService = configService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var hps = _configService.Get<HpsCfg>("hps");
            var hps2 = ConfigContainer.Instance.Get<HpsCfg>("hps");
            var hps3 = _configService.HpsCfg;


            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
