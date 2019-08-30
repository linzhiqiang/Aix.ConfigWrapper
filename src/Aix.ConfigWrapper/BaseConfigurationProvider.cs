using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aix.ConfigWrapper
{
    public abstract class BaseConfigurationProvider : ConfigurationProvider
    {
        public BaseConfigurationProvider()
        {
            ConfigContainer.Instance.OnConfigChange += Instance_OnConfigChange;
        }
        private void Instance_OnConfigChange(ConfigChangeInfo obj)
        {
            Reload(obj.GroupCode, obj.Key, obj.Value);
           
        }

        public virtual void Reload(string groupCode, string key, string value)
        {
        }

        protected void ConvertToJsonConfiguration(IDictionary<string, string> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            int index = 0;
            foreach (var item in data)
            {
                index++;
                sb.AppendFormat("\"{0}\":{1}", item.Key, item.Value);
                if (index != data.Count)
                {
                    sb.Append(",");
                }
                sb.AppendLine();
            }
            sb.Append(" }");


            byte[] array = Encoding.UTF8.GetBytes(sb.ToString());
            using (MemoryStream stream = new MemoryStream(array))
            {
                this.Data = JsonConfigurationFileParser.Parse(stream);
            }
            this.OnReload();
        }

        protected void AddData(IDictionary<string, string> data, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            if (string.IsNullOrWhiteSpace(value)) return;

            key = key.ToLower();
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }
    }
}
