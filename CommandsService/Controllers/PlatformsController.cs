using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepository repository;
        private readonly IMapper mapper;

        public PlatformsController(ICommandRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetAllPlatforms()
        {
            var platforms = repository.GetAllPlatforms();

            return Ok(mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
        }
    }
}