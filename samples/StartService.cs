using Aix.ConfigWrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
        private ConfigChangeTest _configChangeTest;
        public StartService(IConfiguration configuration, ConfigChangeTest configChangeTest)
        {
            _configuration = configuration;
            _configChangeTest = configChangeTest;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var redisString = _configuration.GetValue<string>("redis:configuration");
            var redisConfig = _configuration.GetSection("redis").Get<RedisConfig>();
            _configChangeTest.Test();
            ConfigContainer.Instance.Change(new ConfigChangeInfo {  GroupCode= "common", Key= "redis",Value= "{configuration:\"110.240.225.136\"}" });

            _configChangeTest.Test();
            redisString = _configuration.GetValue<string>("redis:configuration");
            redisConfig = _configuration.GetSection("redis").Get<RedisConfig>();
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

    public class ConfigChangeTest
    {
        IOptionsMonitor<RedisConfig> _options;

        IOptionsSnapshot<RedisConfig> _options2;
        public ConfigChangeTest(IOptionsMonitor<RedisConfig> options, IOptionsSnapshot<RedisConfig> options2)
        {
            _options = options;
            _options2 = options2;
        }

        public void Test()
        {
            var value = _options.CurrentValue;
            var value2 = _options2.Value;
        }
    }

    public class RedisConfig//:IOptions<RedisConfig>
    {
        public string Configuration { get; set; } = "abc";

     //   public RedisConfig Value =>this;
    }
}
