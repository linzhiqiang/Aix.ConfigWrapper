using Dapper;
using System;
using System.Linq;

namespace Aix.ConfigWrapper.DB
{
    public class DBConfigurationProvider : BaseConfigurationProvider
    {
        DBConfigurationOption _option;
        public DBConfigurationProvider(DBConfigurationOption option)
        {
            _option = option;
        }
        public override void Load()
        {
            try
            {
                GetAndAdd();
                // Console.WriteLine("DB配置加载成功");
                ConvertToJsonConfiguration();
            }
            catch (Exception ex)
            {
                throw new Exception("DB配置加载异常", ex);
            }
        }

        private void GetAndAdd()
        {
            if (_option.Groups == null || _option.Groups.Length == 0) throw new Exception("Groups为空");

            using (var connection =    ConnectionFactory.Instance.GetConnectionFactory().CreateConnection(_option.ConfigConnectionString))
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
                        foreach (var g in group)
                        {
                            AddData(g.Key, g.Value);
                        }
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
}
