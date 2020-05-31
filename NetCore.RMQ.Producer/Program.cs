using RabbitMQ.Client;
using System;
using System.Text;

namespace NetCore.RMQ.Producer
{
    class Program
    {
        private const string QUEUE_NAME = "NetCore.RMQ.Queue";
        private const string HOSTNAME = "localhost";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = HOSTNAME };
            
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Will create queue if not exists
                channel.QueueDeclare(queue: QUEUE_NAME,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "This is a test message";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: QUEUE_NAME,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
