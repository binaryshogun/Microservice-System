using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Services.AsyncMessaging;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository repository;
        private readonly IMapper mapper;
        private readonly IMessageBusClient messageBusClient;

        public PlatformsController(
            IPlatformRepository repository, 
            IMapper mapper,
            IMessageBusClient messageBusClient)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("Getting Platforms...");

            var platforms = repository.GetAllPlatforms();
            
            return Ok(mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
        }

        [HttpGet("{id}")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platform = repository.GetPlatformById(id);

            if (platform is not null)
            {
                return Ok(mapper.Map<PlatformReadDTO>(platform));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformReadDTO> CreatePlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platform = mapper.Map<Platform>(platformCreateDTO);

            repository.CreatePlatform(platform);
            repository.SaveChanges();

            var platformReadDTO = mapper.Map<PlatformReadDTO>(platform);

            // Send Async Message
            try
            {
                var platformPublishedDTO = mapper.Map<PlatformPublishedDTO>(platformReadDTO);
                platformPublishedDTO.Event = "Platform_Published";
                
                messageBusClient.PublishNewPlatform(platformPublishedDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send message asynchronously: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }
    }
}