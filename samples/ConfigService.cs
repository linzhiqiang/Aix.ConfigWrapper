
using Aix.ConfigWrapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    public interface IConfigService
    {
        T Get<T>(string key);

        KDNiaoConfig KDNiaoConfig { get; }
    }

  public  class ConfigService: IConfigService
    {
        public T Get<T>(string key)
        {
            return ConfigContainer.Instance.Get<T>(key);
        }

        #region 配置项
        public KDNiaoConfig KDNiaoConfig
        {
            get { return Get<KDNiaoConfig>("kdniao"); }
        }

        #endregion
    }

    public class KDNiaoConfig
    {
        public string Url { get; set; }
        public string AppId { get; set; }

        public string AppSecret { get; set; }
    }
}


