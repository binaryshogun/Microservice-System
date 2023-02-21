using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace PlatformService.Services.SyncMessaging.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDTO platformReadDTO)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platformReadDTO),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(
                configuration.GetSection("CommandsService").GetValue<string>("Post"), 
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("Sync POST to CommandService failed!");
            }
        }
    }
}