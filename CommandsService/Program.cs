using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.Services.AsyncMessaging;
using CommandsService.Services.SyncMessaging.gRPC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseInMemoryDatabase("InMemoryDb");
});

builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
 
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

PrepareDatabase.PopulateData(app);

app.Run();
