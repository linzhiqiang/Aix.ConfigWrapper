using Aix.ConfigWrapper.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Aix.ConfigWrapper
{
    public class ConfigFileParserTools
    {
        public static IConfiguration ParseConfiguration(string[] configFiles)
        {
            var builder = new ConfigurationBuilder();
            foreach (var item in configFiles)
            {
                var path = item;
                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), path)))
                {
                    builder.AddJsonFile(path);
                }
            }
            return builder.Build();
        }

        public static IDictionary<string, string> ParseKV(string[] configFiles)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in configFiles)
            {
                var path = item;
                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), path)))
                {
                    string jsonStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path));
                    var rootJson = JObject.Parse(jsonStr);
                    foreach (var child in rootJson)
                    {
                        var strValue = JsonUtils.ToJson(child.Value);

                        if (result.ContainsKey(child.Key))
                        {
                            result[child.Key] = strValue;
                        }
                        else
                        {
                            result.Add(child.Key, strValue);
                        }
                    }
                }
            }

            return result;
        }
    }
}
