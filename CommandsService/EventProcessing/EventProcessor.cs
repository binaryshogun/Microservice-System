using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IMapper mapper;

        public EventProcessor(
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.PlatformPublished:
                    break;
                case EventType.Undetermined:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage)?.Event;
            
            return eventType switch
            {
                "Platform_Published" => EventType.PlatformPublished,
                _ => EventType.Undetermined
            };
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

                var platformPublishedDTO = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);

                try
                {
                    var platform = mapper.Map<Platform>(platformPublishedDTO);

                    if (!repository.ExternalPlatformExists(platform.Id))
                    {
                        repository.CreatePlatform(platform);
                        repository.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Platform already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not add platform to database: {ex.Message}");
                }
            }
        }
    }
}