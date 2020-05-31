﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace NetCore.RMQ.Common
{
    public class RmqClient : IDisposable
    {
        public string Hostname { get; }
        public string Queue { get; }

        public event MessageReceivedEventHandler MessageReceived;
        public delegate void MessageReceivedEventHandler(string message);

        private IConnection consumerConnection;
        private IModel consumerChannel;

        public RmqClient(string hostname, string queue)
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
                DeclareQueue(consumerChannel);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: Queue,
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void StartMessageConsumer()
        {
            var factory = new ConnectionFactory() { HostName = Hostname };

            consumerConnection = factory.CreateConnection();
            consumerChannel = consumerConnection.CreateModel();

            DeclareQueue(consumerChannel);

            var consumer = new EventingBasicConsumer(consumerChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                this.MessageReceived.Invoke(message);
            };

            consumerChannel.BasicConsume(queue: Queue,
                                     autoAck: true,
                                     consumer: consumer);
        }

        private void DeclareQueue(IModel channel)
        {
            channel.QueueDeclare(queue: Queue,
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);
        }

        public void StopMessageConsumer()
        {
            consumerChannel.Close();
            consumerConnection.Close();
        }

        public void Dispose()
        {
            StopMessageConsumer();
        }
    }
}