using CommandsService.Models;
using CommandsService.Services.SyncMessaging.gRPC;

namespace CommandsService.Data
{
    public static class PrepareDatabase
    {
        public static void PopulateData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetRequiredService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                AddPlatforms(serviceScope.ServiceProvider.GetRequiredService<ICommandRepository>(), platforms);
            }
        }

        private static void AddPlatforms(ICommandRepository repository, IEnumerable<Platform> platforms)
        {
            foreach (Platform platform in platforms)
            {
                if (!repository.ExternalPlatformExists(platform.PlatformId))
                {
                    repository.CreatePlatform(platform);
                }
            }

            repository.SaveChanges();
        }
    }
}