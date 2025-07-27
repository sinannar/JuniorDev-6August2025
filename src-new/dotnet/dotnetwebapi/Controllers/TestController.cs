using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace dotnetwebapi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController(
    ILogger<TestController> logger
    , AppDbContext appDbContext
    , QueueClient queueClient
    , IDistributedCache cache
    ) : ControllerBase
{


    //
    // Storage Queue
    //
    [HttpPut]
    public async Task SendMessageToQueue(string message)
    {
        await queueClient.CreateIfNotExistsAsync().ConfigureAwait(false);
        await queueClient.SendMessageAsync(message).ConfigureAwait(false);
        logger.LogInformation("SendMessageToQueue called with message: {Message}", message);
    }

    [HttpGet]
    public async Task<string> ReadMessageFromQueue()
    {
        var response = await queueClient.ReceiveMessageAsync().ConfigureAwait(false);
        if (response.Value == null)
        {
            throw new InvalidOperationException("No messages available in the queue.");
        }

        var message = response.Value.MessageText;
        logger.LogInformation("ReadMessageFromQueue called, received message: {Message}", message);
        return message;
    }

    //
    // Redis
    //
    [HttpPut]
    public async Task SetCache(string key, string value)
    {
        await cache.SetStringAsync(key, value).ConfigureAwait(false);
        logger.LogInformation("SetCache called with key: {Key}, value: {Value}", key, value);
    }

    [HttpGet]
    public async Task<string> GetCache(string key)
    {
        var value = await cache.GetStringAsync(key).ConfigureAwait(false);
        if (value == null)
        {
            throw new InvalidOperationException($"No cache entry found for key: {key}");
        }
        logger.LogInformation("GetCache called with key: {Key}, retrieved value: {Value}", key, value);
        return value;
    }

    //
    // SQL
    //
    [HttpPut]
    public async Task InsertData(string name, string description)
    {
        logger.LogInformation("InsertData called with name: {Name}, description: {Description}", name, description);

        var data = new TestData
        {
            Name = name + DateTimeOffset.Now.ToString(),
            Description = description + DateTimeOffset.Now.ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        appDbContext.TestDatas.Add(data);
        await appDbContext.SaveChangesAsync();
        logger.LogInformation("Data inserted with Id: {Id}", data.Id);
    }

    [HttpGet]
    public async Task<TestData> ReadData(int id)
    {
        logger.LogInformation("ReadData called with id: {Id}", id);
        var data = await appDbContext.TestDatas.FindAsync(id);
        if (data == null)
        {
            throw new InvalidOperationException($"No data found with Id: {id}");
        }
        logger.LogInformation("Data retrieved with Id: {Id}, Name: {Name}, Description: {Description}", data.Id, data.Name, data.Description);
        return data;
    }
}
