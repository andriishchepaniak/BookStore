using BookStoreClient;
using BookStoreModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;

namespace BookStoreConsumer
{
    public class Consumer
    {
        public static void ReceiveData()
        {
            var authorService = new AuthorClientService();
            var bookService = new BookClientService();

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("test",
                durable: true,
                autoDelete: false,
                exclusive: false,
                arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (sender, e) =>
                {
                    var body = e.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    var data = JsonConvert.DeserializeObject<BooksAndAuthors>(message);

                    if(data != null)
                    {
                        Console.WriteLine("Message received");
                        Console.WriteLine("\n");
                        for (var i = 0; i < data.Authors.Count; i++)
                        {
                            var author = await authorService.AddAuthor(data.Authors[i]);
                            data.Books[i].AuthorId = author.Id;
                            var book = await bookService.AddBook(data.Books[i]);
                        }
                    }
                };

                channel.BasicConsume(queue: "test",
                                 autoAck: true,
                                 consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }
    }
}
