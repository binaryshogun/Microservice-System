using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext context;

        public CommandRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.PlatformId = platformId;
            context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            
            context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return context.Platforms.ToList();
        }

        public Command? GetCommand(int platformId, int commandId)
        {
            return context.Commands.FirstOrDefault(c => 
                c.PlatformId == platformId && c.Id == commandId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(p => p.PlatformId);
        }

        public bool PlatformExists(int platformId)
        {
            return context.Platforms.Find(platformId) is not null;
        }

        public bool SaveChanges()
        {
            return (context.SaveChanges() >= 0);
        }
    }
}