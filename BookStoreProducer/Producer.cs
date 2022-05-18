using BookStoreModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace BookStoreProducer
{
    public class Producer
    {
        public static void SendObjectMessage(object message)
        {
            var stringMessage = JsonConvert.SerializeObject(message);
            SendMessage(stringMessage);
        }
        private static void SendMessage(string data)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("test",
                durable: true,
                autoDelete: false,
                exclusive: false,
                arguments: null);


                var body = Encoding.UTF8.GetBytes(data);

                channel.BasicPublish(exchange: "",
                                     routingKey: "test",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", data);
            }
        }
    }
}
