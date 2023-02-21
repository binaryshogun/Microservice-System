using PlatformService.DTOs;

namespace PlatformService.Services.Async
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO);
    }
}