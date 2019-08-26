using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Aix.ConfigWrapper.DB
{
    public class ConnectionFactory
    {
        public IConnectionFactory DefaultFactory = null;

        public static ConnectionFactory Instance = new ConnectionFactory();

        private ConnectionFactory() { }

        public IConnectionFactory GetConnectionFactory()
        {
            if (DefaultFactory == null) throw new Exception("请配置IConnectionFactory");
            return DefaultFactory;
        }

    }

    public interface IConnectionFactory
    {
        IDbConnection CreateConnection(string connectionString);
    }
}
