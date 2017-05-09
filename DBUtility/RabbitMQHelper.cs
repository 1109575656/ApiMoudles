using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace DBUtility
{
    public class RabbitMQHelper
    {

        public string HostName { get; private set; }

        public string Password { get; private set; }

        public string UserName { get; private set; }

        public int Port { get; private set; }

        public RabbitMQHelper(string hostName, string password, string userName, int port)
        {
            this.HostName = hostName;
            this.Password = password;
            this.UserName = userName;
            this.Port = port;
        }
        /// <summary>
        /// 点对点发送到消息队列
        /// </summary>
        /// <param name="mqName"></param>
        /// <param name="parameter"></param>
        public void SendToMQ(string mqName, object parameter)
        {
            var factory = new ConnectionFactory { HostName = this.HostName, Password = this.Password, UserName = this.UserName, Port = this.Port };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(mqName, false, false, false, null);
                    string message = JsonConvert.SerializeObject(parameter);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", mqName, null, body);
                }
            }
        }

        private void ReadToMQ(string mqName, object parameter)
        {
            var factory = new ConnectionFactory { HostName = this.HostName, Password = this.Password, UserName = this.UserName, Port = this.Port };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(mqName, false, false, false, null);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(mqName, true, consumer);
                    //监听中
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = ea.Body;
                        //队列中的消息
                        var message = Encoding.UTF8.GetString(body);
                    }
                }
            }
        }
    }
}
