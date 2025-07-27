using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sqlserver = builder.AddSqlServer("sqlserver");
var appDb = sqlserver.AddDatabase("appDb");

var redis = builder.AddRedis("redis");

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();
var queues = storage.AddQueues("test-queue");

var apiapp = builder.AddProject<dotnetwebapi>("webapi")
    .WithEnvironment("ASPIRED_BOOTSTRAP", "true")
    .WithReference(appDb).WaitFor(appDb)
    .WithReference(queues).WaitFor(queues)
    .WithReference(redis).WaitFor(redis);

var spa = builder.AddNpmApp("spa", "../../angular")
    .WithReference(apiapp).WaitFor(apiapp)
    .WithUrl("http://localhost:4200")
    .WithHttpEndpoint(env: "PORT");


builder.Build().Run();
