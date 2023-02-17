using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepository
    {
        public bool SaveChanges();

        public IEnumerable<Platform> GetAllPlatforms();
        public Platform? GetPlatformById(int id);
        public void CreatePlatform(Platform platform);
    }
}