using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class SeedData
    {
        public static void PopulateData(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                AddDefaultEntries(serviceScope.ServiceProvider.GetRequiredService<AppDbContext>(), isProduction);
            }
        }

        private static void AddDefaultEntries(AppDbContext context, bool isProduction)
        {
            if (isProduction && context.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                    Console.WriteLine($"Migrations applied successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to apply migrations: {ex.Message}");
                }
            }

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