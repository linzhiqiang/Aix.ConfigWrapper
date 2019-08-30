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
            services.Configure<RedisConfig>(context.Configuration.GetSection("redis"));
            services.AddSingleton<ConfigChangeTest>();
            services.AddHostedService<StartService>();
        }
    }
}
