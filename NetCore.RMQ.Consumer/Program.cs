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
            var rmqMessageConsumer = new RmqMessageConsumer(HOSTNAME, QUEUE_NAME);
            
            rmqMessageConsumer.MessageReceived += (message) => Console.WriteLine($"Message received: {message}");
            rmqMessageConsumer.StartMessageConsumer();

            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();

            rmqMessageConsumer.StopMessageConsumer();
        }
    }
}