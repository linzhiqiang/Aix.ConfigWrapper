using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aix.ConfigWrapper
{
    public abstract class BaseConfigurationProvider : ConfigurationProvider
    {
        private IDictionary<string, string> _myData;
        protected IDictionary<string, string> MyData
        {
            get
            {
                if (_myData == null) _myData = new Dictionary<string, string>();
                return _myData;
            }
            set { _myData = value; }
        }

        protected void AddData(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;

            key = key.ToLower();
            if (MyData.ContainsKey(key))
            {
                MyData[key] = value;
            }
            else
            {
                MyData.Add(key, value);
            }
        }

        protected void ConvertToJsonConfiguration()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            int index = 0;
            foreach (var item in this.MyData)
            {
                index++;
                sb.AppendFormat("\"{0}\":{1}", item.Key, item.Value);
                if (index != this.MyData.Count)
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
        }
    }
}
