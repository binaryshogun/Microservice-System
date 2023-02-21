using System.Text;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.Services.AsyncMessaging
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IEventProcessor eventProcessor;
        private IConnection? connection;
        private IModel? channel;
        private string? queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            this.configuration = configuration;
            this.eventProcessor = eventProcessor;

            try
            {
                InitializeRabbitMQ();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not establish connection with message bus: {ex.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ModuleHandle, e) =>
            {
                var body = e.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                eventProcessor.ProcessEvent(notificationMessage);
            };

            channel.BasicConsume(queueName, true, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if(channel?.IsOpen ?? false)
            {
                channel?.Close();
                connection?.Close();
            }

            base.Dispose(); 
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = configuration["RabbitMQHost"], 
                Port = configuration.GetValue<int>("RabbitMQPort")
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare("trigger", ExchangeType.Fanout);

            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "trigger", "");

            Console.WriteLine("Message bus connection established.");

            connection.ConnectionShutdown += RabbitMQ_ConnectionShitdown;
        }

        private void RabbitMQ_ConnectionShitdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Message bus connection has been shutdown.");
        }
    }
}