
using Aix.ConfigWrapper;
using Aix.ConfigWrapper.ConsulEx;
using Aix.ConfigWrapper.MySqlEx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

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
                      config.AddJsonFile("config/dotbpe.json", optional: true);
                      config.AddJsonFile($"config/dotbpe.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);

                      var configFiles = new string[] { "config/appsettings.json", $"config/appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json" };
                      var configuration = ConfigFileParserTools.ParseConfiguration(configFiles);

                      var type = 3;//1=配置文件 2=db   3=consul  
                      switch (type)
                      {
                          case 1:
                              config.AddFileConfiguration(new FileConfigurationOption { ConfigFiles = configFiles , ConfigurationBuilder= config });
                              break;
                          case 2:
                              var adminDBConnectionString = configuration["connectionStrings:admin"];
                              config.AddDBMysqlConfiguration(new DBConfigurationOption
                              {
                                  ConfigConnectionString = adminDBConnectionString,
                                  AppCode = "wenbo",
                                  Groups = new string[] { "common", "ecshop" }
                              });
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
}
