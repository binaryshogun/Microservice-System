using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Services.Sync.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository repository;
        private readonly IMapper mapper;
        private readonly ICommandDataClient commandDataClient;

        public PlatformsController(
            IPlatformRepository repository, 
            IMapper mapper, 
            ICommandDataClient commandDataClient)
        {
            this.commandDataClient = commandDataClient;
            this.mapper = mapper;
            this.repository = repository;
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
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platform = mapper.Map<Platform>(platformCreateDTO);

            repository.CreatePlatform(platform);
            repository.SaveChanges();

            var platformReadDTO = mapper.Map<PlatformReadDTO>(platform);

            try 
            {
                await commandDataClient.SendPlatformToCommand(platformReadDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send sync POST request to Commands Service: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }
    }
}