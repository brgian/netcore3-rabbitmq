using NetCore.RMQ.Common;
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
            var rmqClient = new RmqClient(HOSTNAME, QUEUE_NAME);

            rmqClient.MessageReceived += (message) => Console.WriteLine($"Message received: {message}");
            rmqClient.StartMessageConsumer();

            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }
    }
}
