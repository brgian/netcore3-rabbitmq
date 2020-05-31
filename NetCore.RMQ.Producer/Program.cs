using NetCore.RMQ.Common;
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
            var rmqClient = new RmqClient(HOSTNAME, QUEUE_NAME);

            var exit = false;
            var c = 1;

            Console.WriteLine("Press S to send another message, press E to exit");

            while (!exit)
            {
                var keyInfo = Console.ReadKey();

                if (keyInfo.KeyChar == 's')
                {
                    rmqClient.Send($"Test message {c}");
                    c++;
                }
                else if (keyInfo.KeyChar == 'e')
                    exit = true;
            }
        }
    }
}
