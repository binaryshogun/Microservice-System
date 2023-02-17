using PlatformService.Models;

namespace PlatformService.Data
{
    public static class SeedData
    {
        public static void PopulateData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                AddDefaultEntries(serviceScope.ServiceProvider.GetRequiredService<AppDbContext>());
            }
        }

        private static void AddDefaultEntries(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding data...");

                context.Platforms.AddRange(
                    new Platform() { Name = ".NET", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();

                Console.WriteLine("Data populated.");
            }
            else
            {
                Console.WriteLine("The data is already in the database.");
            }
        }
    }
}