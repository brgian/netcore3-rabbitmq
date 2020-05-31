using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace NetCore.RMQ.Consumer
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: QUEUE_NAME,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
