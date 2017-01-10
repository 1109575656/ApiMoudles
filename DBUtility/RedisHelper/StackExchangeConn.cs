using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility.RedisHelper
{
    public class StackExchangeConn
    {
        private static ConnectionMultiplexer _connection;
        private static readonly object SyncObject = new object();
        public static ConnectionMultiplexer GetFactionConn
        {
            get
            {
                if (_connection == null || !_connection.IsConnected)
                {
                    lock (SyncObject)
                    {
                        try
                        {
                            var configurationOptions = new ConfigurationOptions()
                            {
                                Password = "Redis密码",
                                EndPoints = { { "ip（192.168.100.37）", Convert.ToInt32("端口（8888）") } }
                            };
                            //"192.168.100.37,password=myRedis";
                            _connection = ConnectionMultiplexer.Connect(configurationOptions);
                        }
                        catch (Exception ex)
                        {
                            _connection = null;
                        }
                        
                    }
                }
                return _connection;
            }
        }
    }
}
