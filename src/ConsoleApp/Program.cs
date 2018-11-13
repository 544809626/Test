using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
   
    public class Program
    {
       
        /// <summary>
        /// 连接配置
        /// </summary>
        private static readonly ConnectionFactory rabbitMqFactory = new ConnectionFactory()
        {
            HostName = "192.168.1.215",
            UserName = "yangbing",
            Password = "yangbing",
            Port = 5672,
            VirtualHost = "yangbingVirtualHost"
        };
        /// <summary>
        /// 路由名称
        /// </summary>
        const string ExchangeName = "yangbing.exchange";

        //队列名称
        const string QueueName = "yangbing.queue";

        /// <summary>
        /// 路由名称
        /// </summary>
       // const string TopExchangeName = "topic.cyw.exchange";

        //队列名称
      //  const string TopQueueName = "topic.cyw.queue";
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            DirectExchangeSendMsg();
            // TopicExchangeSendMsg();
            Console.WriteLine("按任意值，退出程序");
            Console.ReadKey();
        }
        public static void DirectExchangeSendMsg()
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, "direct", durable: true, autoDelete: false, arguments: null);
                    channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: null);
                    channel.QueueBind(QueueName, ExchangeName, routingKey: QueueName);

                    var props = channel.CreateBasicProperties();
                    props.Persistent = true;
                    var vadata = Console.ReadLine();
                    while (vadata != "exit")
                    {
                        var msgBody = Encoding.UTF8.GetBytes(vadata);
                        channel.BasicPublish(exchange: ExchangeName, routingKey: QueueName, basicProperties: props, body: msgBody);
                        Console.WriteLine(string.Format("***发送时间:{0}，发送完成，输入exit退出消息发送",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        vadata = Console.ReadLine();
                    }
                }
            }
        }
    }
}
