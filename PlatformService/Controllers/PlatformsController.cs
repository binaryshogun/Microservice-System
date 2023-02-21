using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository repository;
        private readonly IMapper mapper;

        public PlatformsController(IPlatformRepository repository, IMapper mapper)
        {
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
        public ActionResult<PlatformReadDTO> CreatePlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platform = mapper.Map<Platform>(platformCreateDTO);

            repository.CreatePlatform(platform);
            repository.SaveChanges();

            var platformReadDTO = mapper.Map<PlatformReadDTO>(platform);

            return CreatedAtAction(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }
    }
}