using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace NetCore.RMQ.Common
{
    public class RmqMessageSender
    {
        public string Hostname { get; }
        public string Queue { get; }

        public RmqMessageSender(string hostname, string queue)
        {
            Hostname = hostname;
            Queue = queue;
        }

        public void Send(string message)
        {
            var factory = new ConnectionFactory() { HostName = Hostname };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                DeclareQueue(channel);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: Queue,
                                     basicProperties: null,
                                     body: body);
            }
        }

        private void DeclareQueue(IModel channel)
        {
            channel.QueueDeclare(queue: Queue,
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);
        }
    }
}