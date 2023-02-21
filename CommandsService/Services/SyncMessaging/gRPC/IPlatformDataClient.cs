using CommandsService.Models;

namespace CommandsService.Services.SyncMessaging.gRPC
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}