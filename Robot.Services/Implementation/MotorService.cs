using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Robot.Infrastructure.Settings;
using Robot.L298N;
using Robot.Models;
using Robot.Services.Interfaces;
using System.Device.Gpio;
using Robot.Services.Mocks;
using Robot.Services.Helpers;

namespace Robot.Services.Implementation;

/// <summary>
/// Motor Service Class
/// </summary>
public class MotorService : IMotorService, IDisposable
{
    private readonly L298NDriver _motor1;
    private readonly L298NDriver _motor2;
    private readonly ILogger<MotorService> _logger;
    private bool _disposeDrivers = false;

    /// <summary>
    /// Instanciates new Motor Service Class
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public MotorService(ILogger<MotorService> logger, IConfiguration configuration)
    {
        _logger = logger;
        var gpio = GetGpio(configuration);
        var controller = GetGpioController(configuration);
        _motor1 = new L298NDriver(gpio.PWM1, gpio.Pin1, gpio.Pin2, controller);
        _motor2 = new L298NDriver(gpio.PWM2, gpio.Pin3, gpio.Pin4, controller);

    }

    /// <summary>
    /// Set Motors direction
    /// </summary>
    public RequestDirection SetDirection(RequestDirection go)
    {
        if (go.Angle.Equals("up") && go.Direction.Equals("FORWARD")) Forward();
        if (go.Angle.Equals("up") && go.Direction.Equals("RIGHT")) Right();
        if (go.Angle.Equals("up") && go.Direction.Equals("LEFT")) Left();
        if (go.Angle.Equals("down")) Reverse();

        return go;
    }

    /// <summary>
    /// Forward
    /// </summary>
    private void Forward()
    {
        _motor1.Forward();
        _motor2.Forward();
    }

    /// <summary>
    /// Reverse
    /// </summary>
    private void Reverse()
    {
        _motor1.Reverse();
        _motor2.Reverse();
    }

    /// <summary>
    /// Stop
    /// </summary>
    public void Stop()
    {
        _motor1.Stop();
        _motor2.Stop();
    }

    /// <summary>
    /// Gets or sets the speed of the motor.
    /// Speed is a value from -1 to 1.
    /// 1 means maximum speed, signed value changes the direction.
    /// </summary>
    /// <param name="speed">selected speed </param>
    public void SetSpeed(double speed)
    {
        JsonFile.AddOrUpdateAppSetting("Speed", speed);
        _motor1.Speed = speed;
        _motor2.Speed = speed;
    }


    /// <summary>
    /// Left
    /// </summary>
    private void Left()
    {
        _motor1.Stop();
        _motor2.Forward();
    }

    /// <summary>
    /// Right
    /// </summary>
    private void Right()
    {
        _motor1.Forward();
        _motor2.Stop();
    }

    /// <summary>
    /// Dispose Motor Driver
    /// </summary>
    public void Dispose()
    {
        Stop();
        _logger.LogInformation("Motor Drivers disposed");
        if (_disposeDrivers)
        {
            _disposeDrivers = _motor1.Dispose(_disposeDrivers);
            _motor2.Dispose(_disposeDrivers);
        }
        // Exit application
        Environment.Exit(-1);
    }

    /// <summary>
    /// Get gpio from appSettings config
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static GPIO GetGpio(IConfiguration configuration)
    {
        var appSettings = configuration.Get<AppSettings>();
        var gpio = appSettings!.GPIO;
        return gpio;
    }

    /// <summary>
    /// Get gpio controller if is dev or Prod env
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static GpioController GetGpioController(IConfiguration configuration)
    {
        var appSettings = configuration.Get<AppSettings>();

        return appSettings!.IsDevelopment ? new GpioController(PinNumberingScheme.Logical,new GpioDriverMock()) : new GpioController();
    }
}




