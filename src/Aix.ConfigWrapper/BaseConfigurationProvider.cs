using Microsoft.Extensions.Configuration;

namespace Aix.ConfigWrapper
{
    public abstract class BaseConfigurationProvider : ConfigurationProvider
    {
        public void AddData(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;

            key = key.ToLower();
            if (Data.ContainsKey(key))
            {
                Data[key] = value;
            }
            else
            {
                Data.Add(key, value);
            }
            ConfigContainer.Instance.Set(key, value);
        }
    }
}
