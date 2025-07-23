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
    public async Task<IActionResult> SendMessageToQueue(string message)
    {
        await queueClient.CreateIfNotExistsAsync().ConfigureAwait(false);
        await queueClient.SendMessageAsync(message + DateTimeOffset.Now.ToString()).ConfigureAwait(false);
        logger.LogInformation("SendMessageToQueue called with message: {Message}", message);
        return Ok($"SendMessageToQueue executed successfully with message: {message}");
    }

    [HttpGet]
    public async Task<IActionResult> ReadMessageFromQueue()
    {
        var response = await queueClient.ReceiveMessageAsync().ConfigureAwait(false);
        if (response.Value == null)
        {
            return NotFound("No messages in the queue.");
        }

        var message = response.Value.MessageText;
        logger.LogInformation("ReadMessageFromQueue called, received message: {Message}", message);
        return Ok($"ReadMessageFromQueue executed successfully with message: {message}");
    }

    //
    // Redis
    //
    [HttpPut]
    public async Task<IActionResult> SetCache(string key, string value)
    {
        await cache.SetStringAsync(key, value).ConfigureAwait(false);
        return Ok($"SetCache executed successfully for key: {key}");
    }

    [HttpGet]
    public async Task<IActionResult> GetCache(string key)
    {
        var value = await cache.GetStringAsync(key).ConfigureAwait(false);
        if (value == null)
        {
            return NotFound($"No cache entry found for key: {key}");
        }
        return Ok($"GetCache executed successfully for key: {key} with value: {value}");
    }

    //
    // SQL
    //
    [HttpPut]
    public async Task<IActionResult> InsertData(string name, string description)
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

        return Ok($"InsertData executed successfully for name: {name} with Id: {data.Id}");
    }

    [HttpGet]
    public async Task<IActionResult> ReadData(int id)
    {
        logger.LogInformation("ReadData called with id: {Id}", id);
        var data = await appDbContext.TestDatas.FindAsync(id);
        if (data == null)
        {
            return NotFound($"No data found with Id: {id}");
        }
        return Ok($"ReadData executed successfully for Id: {id} with Name: {data.Name}, Description: {data.Description}");
    }
}
