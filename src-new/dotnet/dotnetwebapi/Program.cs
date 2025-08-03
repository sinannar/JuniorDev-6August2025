using Azure.Storage.Queues;
using dotnetwebapi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Allow CORS from any origin
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var isAspiredBootstrap = Environment.GetEnvironmentVariable("ASPIRED_BOOTSTRAP") == "true";
if (isAspiredBootstrap)
{
    builder.AddServiceDefaults();
    builder.AddSqlServerDbContext<AppDbContext>("appDb");
    builder.AddRedisDistributedCache(connectionName: "redis");
    builder.AddAzureQueueServiceClient("test-queue");
    builder.Services.AddSingleton(sp =>
    {
        var qService = sp.GetRequiredService<QueueServiceClient>();
        return qService.GetQueueClient("test-queue");
    });
}
else
{
    var sqlConnectionString = builder.Configuration.GetConnectionString("Sql");
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
    var storageConnectionString = builder.Configuration.GetConnectionString("Storage");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnectionString));
    builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnectionString; });
    builder.Services.AddScoped(_ => new QueueClient(storageConnectionString, "test-queue"));
}


builder.Services.AddOpenApi();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.MapOpenApi();
app.MapDefaultEndpoints();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();