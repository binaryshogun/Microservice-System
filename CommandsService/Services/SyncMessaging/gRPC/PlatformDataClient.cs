using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.Services.SyncMessaging.gRPC
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            this.configuration = configuration;
            this.mapper = mapper;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            var channel = GrpcChannel.ForAddress(configuration["GrpcPlatform"] ?? "");
            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllRequest();

            try
            {
                var response = client.GetAllPlatforms(request);
                return mapper.Map<IEnumerable<Platform>>(response.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not establish connection with gRPC server: {ex.Message}");
                return new List<Platform>();
            }
        }
    }
}