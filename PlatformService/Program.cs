using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Services.AsyncMessaging;
using PlatformService.Services.SyncMessaging.gRPC;
using PlatformService.Services.SyncMessaging.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register automapper as a service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Adding gRPC to the container
builder.Services.AddGrpc();

// Adding DbContext to the container
if (builder.Environment.IsProduction())
{
    Console.WriteLine("Using SQL Server database");

    // Using SQL Server database in production mode
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        // Connection string using password from user-secrets 'platformdbPassword' secret
        options.UseSqlServer($"{builder.Configuration.GetConnectionString("PlatformsConnection")}"
            + $"Password={Environment.GetEnvironmentVariable("platformsdbPassword")}");
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
// Adding sync messaging http client to the container
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
// Adding async messaging message bus client to the container
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Mapping gRPC service as endpoint
app.MapGrpcService<GrpcPlatformService>();

SeedData.PopulateData(app, app.Environment.IsProduction());

app.Run();
