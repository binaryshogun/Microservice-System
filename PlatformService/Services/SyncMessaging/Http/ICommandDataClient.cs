using PlatformService.DTOs;

namespace PlatformService.Services.SyncMessaging.Http
{
    public interface ICommandDataClient
    {
        public Task SendPlatformToCommand(PlatformReadDTO platformReadDTO);
    }
}