using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository repository;
        private readonly IMapper mapper;

        public CommandsController(ICommandRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDTO>> GetCommandsForPlatform(int platformId)
        {
            if (!repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = repository.GetCommandsForPlatform(platformId);

            return Ok(mapper.Map<IEnumerable<CommandReadDTO>>(commands));
        }

        [HttpGet("{commandId}")]
        public ActionResult<CommandReadDTO> GetCommandForPlatform(int platformId, int commandId)
        {
            if (!repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = repository.GetCommand(platformId, commandId);

            if (command is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CommandReadDTO>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommandForPlatform(int platformId, CommandCreateDTO commandCreateDTO)
        {
            if (!repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            Command command = mapper.Map<Command>(commandCreateDTO);

            repository.CreateCommand(platformId, command);
            repository.SaveChanges();

            var commandReadDTO = mapper.Map<CommandReadDTO>(command);

            return CreatedAtAction(nameof(GetCommandForPlatform), new { platformId, commandId = commandReadDTO.Id }, commandReadDTO);
        }
    }
}