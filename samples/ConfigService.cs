
using Aix.ConfigWrapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    public interface IConfigService
    {
        T Get<T>(string key);

        HpsCfg HpsCfg { get; }
    }

  public  class ConfigService: IConfigService
    {
        public T Get<T>(string key)
        {
            return ConfigContainer.Instance.Get<T>(key);
        }

        #region 配置项
        public HpsCfg HpsCfg
        {
            get { return Get<HpsCfg>("hps"); }
        }

        #endregion
    }

    public class HpsCfg
    {
        public string Url { get; set; }
        public string AppId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

    }
}


