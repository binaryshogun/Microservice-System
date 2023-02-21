using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.Services.AsyncMessaging
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration configuration;
        private readonly IConnection? connection;
        private readonly IModel? channel;

        public MessageBusClient(IConfiguration configuration)
        {
            this.configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQHost"],
                Port = configuration.GetValue<int>("RabbitMQPort")
            };
            try
            {
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();

                channel.ExchangeDeclare("trigger", ExchangeType.Fanout);

                connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("Message bus connection has been established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to the message bus: {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO)
        {
            var message = JsonSerializer.Serialize(platformPublishedDTO);

            if (connection is not null && connection.IsOpen)
            {
                Console.WriteLine("Sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("RabbitMQ connection closed. Could not send message.");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel?.BasicPublish("trigger", "", null, body);
            Console.WriteLine($"Message '{message}' has been sent.");
        }

        public void Dispose()
        {
            if (channel?.IsOpen ?? false)
            {
                channel?.Close();
                connection?.Close();
            }

            Console.WriteLine("MessageBus disposed.");
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ connection has been shutdown.");
        }
    }
}