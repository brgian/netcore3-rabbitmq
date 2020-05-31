using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore.RMQ.Common
{
    public class RmqMessageConsumer : IDisposable
    {
        public string Hostname { get; }
        public string Queue { get; }

        public event MessageReceivedEventHandler MessageReceived;
        public delegate void MessageReceivedEventHandler(string message);

        private IConnection consumerConnection;
        private IModel consumerChannel;

        public bool IsConsuming => consumerConnection.IsOpen && 
            consumerChannel.IsOpen && 
            MessageReceived != null;

        public RmqMessageConsumer(string hostname, string queue, MessageReceivedEventHandler callback)
        {
            Hostname = hostname;
            Queue = queue;
            MessageReceived = callback;
            
            StartMessageConsumer();
        }

        public RmqMessageConsumer(string hostname, string queue)
        {
            Hostname = hostname;
            Queue = queue;
        }

        public void StartMessageConsumer()
        {
            var factory = new ConnectionFactory() { HostName = Hostname };

            consumerConnection = factory.CreateConnection();
            consumerChannel = consumerConnection.CreateModel();

            DeclareQueue(consumerChannel);

            if (MessageReceived != null)
            {
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
            else
                throw new NoMessageReceivedHandlerException();
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
