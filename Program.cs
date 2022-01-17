using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
//using Apache.NMS;
//using Apache.NMS.ActiveMQ.Transport.Tcp;
//using Apache.NMS.ActiveMQ.Transport;
//using Apache.NMS.ActiveMQ;
//using Apache.NMS.ActiveMQ.Util;

namespace ConsoleApp1
{
    class Program
    {
        public static void Receive(string queue)
        {
            using (IConnection connection = GetConnection().CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, true, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    BasicGetResult result = channel.BasicGet(queue, true);
                    if (result != null)
                    {
                        string data =
                        Encoding.UTF8.GetString(result.Body.ToArray());
                        Console.WriteLine(data);
                    }
                }
            }
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Creating Producer Thread");
            Send("IDG", "Hello World!");
            Receive("IDG");
            Console.ReadLine();
        }

        public static void Send(string queue, string data)
        {

            using (IConnection connection = GetConnection().CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, true, false, false, null);
                    channel.BasicPublish(string.Empty, queue, null, System.Text.Encoding.UTF8.GetBytes(data));
                }
            }
        }
        public static ConnectionFactory GetConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amq URL");
            connectionFactory.UserName = "username";
            connectionFactory.Password = "passwprd";
            return connectionFactory;
        }
    }
}
