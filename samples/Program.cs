
using Aix.ConfigWrapper;
using Aix.ConfigWrapper.Consul;
using Aix.ConfigWrapper.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                 .ConfigureHostConfiguration(builder =>
                 {
                     builder.AddEnvironmentVariables(prefix: "Demo_");
                 })
                  .ConfigureAppConfiguration((hostContext, config) =>
                  {
                      config.AddJsonFile("config/appsettings.json", optional: true);
                      config.AddJsonFile($"config/appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);

                      var configFiles = new string[] { "config/appsettings.json", $"config/appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json" };
                    var configuration = ConfigFileParserTools.ParseConfiguration(configFiles);

                      var type = 4;//1=配置文件 2=db   3=consul  
                      switch (type)
                      {
                          case 1:
                              config.AddFileConfiguration(new FileConfigurationOption { ConfigFiles = configFiles, ConfigurationBuilder = config });
                              break;
                          case 2:
                              ConnectionFactory.Instance.DefaultFactory = new MySqlConnectionFactory();
                              config.AddDBConfiguration(configuration.GetSection("configCenter").Get<DBConfigurationOption>());
                              break;
                          case 3:
                              // var consulUrl = configuration["consul:url"];
                              config.AddConsulConfiguration(configuration.GetSection("consul").Get<ConsulConfigurationOption>());
                              break;
                      }

                  })
                 .ConfigureLogging((context, factory) =>
                 {

                 })
                 .ConfigureServices(Startup.ConfigureServices);


            host.RunConsoleAsync().Wait();
            Console.WriteLine("服务已退出");
        }
    }

    public class MySqlConnectionFactory : IConnectionFactory
    {
        public IDbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
