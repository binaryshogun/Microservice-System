using PlatformService.DTOs;

namespace PlatformService.Services.AsyncMessaging
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO);
    }
}