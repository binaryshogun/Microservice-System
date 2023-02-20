using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Services.Sync.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding DbContext to the container
if (builder.Environment.IsProduction())
{
    Console.WriteLine("Using SQL Server database");

    // Using SQL Server database in production mode
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        // Connection string using password from user-secrets 'platformdbPassword' secret
        options.UseSqlServer($"{builder.Configuration.GetConnectionString("PlatformsConnection")}"
            + $"Password={builder.Configuration["platformsdbPassword"]}");
    });
}
else
{
    Console.WriteLine("Using InMemory database");

    // Using InMemoryDatabase in development mode 
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseInMemoryDatabase("InMemoryDb");
    });
}

// Adding repository implementation for IPlatformRepository dependency request
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
// Adding sync http client to the container
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
// Register automapper as a service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

SeedData.PopulateData(app, app.Environment.IsProduction());

app.Run();
