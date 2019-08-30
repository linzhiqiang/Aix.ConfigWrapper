using Dapper;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aix.ConfigWrapper.DB
{
    public class DBConfigurationProvider : BaseConfigurationProvider
    {
        DBConfigurationOption _option;

        IDictionary<string, List<ConfigInfo>> ConfigData = new Dictionary<string, List<ConfigInfo>>();
        public DBConfigurationProvider(DBConfigurationOption option)
        {
            _option = option;
        }
        public override void Load()
        {
            try
            {
                ConfigData.Clear();
                Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                GetData();
                ToJsonConfiguration();
            }
            catch (Exception ex)
            {
                throw new Exception("DB配置加载异常", ex);
            }
        }

        private void ToJsonConfiguration()
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            foreach (var group in ConfigData)
            {
                foreach (var item in group.Value)
                {
                    AddData(data, item.Key, item.Value);
                }
            }

            base.ConvertToJsonConfiguration(data);
        }

        private void GetData()
        {
            if (_option.Groups == null || _option.Groups.Length == 0) throw new Exception("Groups为空");

            using (var connection = ConnectionFactory.Instance.GetConnectionFactory().CreateConnection(_option.ConfigConnectionString))
            {
                var sql = @"SELECT  B.group_code,item.key,item.value 
                FROM config_app   as app 
                INNER JOIN config_group as B on B.app_id =app.app_id
                INNER JOIN config_item as item on item.group_id=B .group_id 
                WHERE app.app_code=@AppCode AND B.group_code in @GroupCodes and item.status=1 ";
                var serch = new { AppCode = _option.AppCode, GroupCodes = _option.Groups };

                var list = connection.Query<ConfigInfo>(sql, serch).ToList();
                var groups = list.GroupBy(x => x.group_code);
                foreach (var item in _option.Groups)
                {
                    var group = groups.FirstOrDefault(x => string.Compare(x.Key, item, true) == 0);
                    if (group != null)
                    {
                        ConfigData.Add(group.Key, group.ToList());
                    }

                    //if (group != null)
                    //{
                    //    foreach (var g in group)
                    //    {
                    //        AddData(g.Key, g.Value);
                    //    }
                    //}
                }
            }
        }

        public override void Reload(string groupCode, string key,string value)
        {
            if (_option.Groups != null && _option.Groups.Contains(groupCode))
            {
                var configInfo = new ConfigInfo {  group_code= groupCode , Key=key, Value= value };
                var isChange = ChangeConfigItem(configInfo);
                if (isChange)
                {
                    Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    ToJsonConfiguration();
                   // ChangeToken.OnChange()
                  // this.OnReload();
                }
            }
        }
        private bool ChangeConfigItem(ConfigInfo configInfo)
        {
            bool isChange = false;
            if (configInfo == null || string.IsNullOrEmpty(configInfo.Value)) return isChange;

            if (ConfigData.ContainsKey(configInfo.group_code))
            {
                var temp = ConfigData[configInfo.group_code].Find(x => x.Key == configInfo.Key);
                if (temp == null)
                {
                    isChange = true;
                    ConfigData[configInfo.group_code].Add(configInfo);
                }
                else
                {
                    if (temp.Value != configInfo.Value)
                    {
                        isChange = true;
                        temp.Value = configInfo.Value;
                    }

                }

            }

            return isChange;
        }

        public  void ReloadBak(string groupCode, string key)
        {
            if (_option.Groups != null && _option.Groups.Contains(groupCode))
            {
                ConfigInfo configInfo = null;
                using (var connection = ConnectionFactory.Instance.GetConnectionFactory().CreateConnection(_option.ConfigConnectionString))
                {
                    var sql = @"SELECT  B.group_code,item.`key`,item.value 
                FROM config_app   as app 
                INNER JOIN config_group as B on B.app_id =app.app_id
                INNER JOIN config_item as item on item.group_id=B .group_id 
                WHERE app.app_code=@AppCode AND B.group_code =@GroupCode AND item.`key`=@Key AND  item.status=1 ";

                    var serch = new { AppCode = _option.AppCode, GroupCode = groupCode, Key = key };
                    configInfo = connection.Query<ConfigInfo>(sql, serch).ToList().FirstOrDefault();
                }

                var isChange = ChangeConfigItem(configInfo);
                if (isChange)
                {
                    ToJsonConfiguration();
                    this.OnReload();
                }
            }

        }
       
    }

    public class ConfigInfo
    {
        public string group_code { get; set; }
        public string Key { get; set; }

        public string Value { get; set; }
    }
}

