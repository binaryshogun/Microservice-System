using PlatformService.DTOs;

namespace PlatformService.Services.Sync.Http
{
    public interface ICommandDataClient
    {
        public Task SendPlatformToCommand(PlatformReadDTO platformReadDTO);
    }
}