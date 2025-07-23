using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace dotnetwebapi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController : ControllerBase
{


    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    //
    // ServiceBus
    //
    [HttpPut]
    public async Task<IActionResult> SendMessageToSB(string message)
    {
        _logger.LogInformation("SendMessageToSB called");
        return Ok("SendMessageToSB executed successfully");
    }

    [HttpGet]
    public async Task<IActionResult> ReadMessageFromSB()
    {
        _logger.LogInformation("ReadMessageFromSB called");
        return Ok("ReadMessageFromSB executed successfully");
    }

    //
    // Redis
    //
    [HttpPut]
    public async Task<IActionResult> SetCache(string key, string value)
    {
        _logger.LogInformation("SetCache called with key: {Key}", key);
        return Ok($"SetCache executed successfully for key: {key}");
    }

    [HttpGet]
    public async Task<IActionResult> GetCache(string key)
    {
        _logger.LogInformation("GetCache called with key: {Key}", key);
        return Ok($"GetCache executed successfully for key: {key}");
    }

    //
    // SQL
    //
    [HttpPut]
    public async Task<IActionResult> InsertData(string data)
    {
        _logger.LogInformation("InsertData called with data: {Data}", data);
        return Ok($"InsertData executed successfully for data: {data}");
    }

    [HttpGet]
    public async Task<IActionResult> ReadData()
    {
        _logger.LogInformation("ReadData called with query: {Query}", query);
        return Ok($"ReadData executed successfully for query: {query}");
    }
    

        
}
