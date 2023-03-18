using Microsoft.AspNetCore.Mvc;
using Robot.Models;
using Robot.Services.Interfaces;

namespace Robot.Api.Controllers;

/// <summary>
/// Motor Controller
/// </summary>
[Route("[controller]")]
[ApiController]
public class MotorController : ControllerBase
{
    /// <summary>
    /// Motors Service control 
    /// </summary>
    private readonly IMotorService _motorService;

    /// <summary>
    /// logger
    /// </summary>
    private readonly ILogger<MotorController> _logger;

    /// <summary>
    /// Instanciates a new instance of <see cref="MotorController"/> class.
    /// </summary>
    /// <param name="motorService"></param>
    /// <param name="logger"></param>
    public MotorController(IMotorService motorService, ILogger<MotorController> logger)
    {
        _motorService = motorService;
        _logger = logger;
    }

    /// <summary>
    /// Set Direction on drive
    /// </summary>
    /// <returns></returns>
    [HttpPost("direction")]
    public IActionResult PostDirection([FromBody] RequestDirection go)
    {
        _logger.LogInformation($"Tank Running!..Angle:{go.Angle} " + $"Direction:{go.Direction}");
        var response = _motorService.SetDirection(go);
        return Ok(response);
    }

    /// <summary>
    /// Stop motors
    /// </summary>
    /// <returns>response</returns>
    [HttpPost("stop")]
    public IActionResult PostStop()
    {
        _logger.LogWarning("Stop");
        _motorService.Stop();

        return Ok(new { response = "stop" });
    }

    /// <summary>
    /// Set speed on motors
    /// </summary>
    /// <param name="speed">selected speed</param>
    /// <returns></returns>
    [HttpPost("speed")]
    public IActionResult PostSpeed([FromBody]RequestSpeed config)
    {
        _logger.LogWarning("Speed changed to " + config.Speed);
        _motorService.SetSpeed(config.Speed);
     

        return Ok("Speed changed to " + config.Speed);
    }

}

