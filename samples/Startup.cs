using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
   public class Startup
    {
        internal static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IConfigService,ConfigService>();
            services.AddHostedService<StartService>();
        }
    }
}
