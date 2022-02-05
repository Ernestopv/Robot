using Microsoft.Extensions.Logging;
using Robot.L298N;
using Robot.Models;
using System.Device.Gpio;

namespace Robot.Services.Implementation;

/// <summary>
/// Motor Service Class
/// </summary>
public class MotorService : IMotorService, IDisposable
{
    private readonly L298NDriver _MOTOR1;
    private readonly L298NDriver _MOTOR2;
    private readonly ILogger<MotorService> _logger;
    private bool _disposeDrivers = false;

    /// <summary>
    /// Instanciates new Motor Service Class
    /// </summary>
    /// <param name="logger"></param>
    public MotorService(ILogger<MotorService> logger)
    {
        _logger = logger;
        var controller = new GpioController();
        _MOTOR1 = new L298NDriver(146, 15, 41, controller);
        _MOTOR2 = new L298NDriver(150, 133, 158, controller);

    }

    /// <summary>
    /// Set Motors direction
    /// </summary>
    public RequestDirection SetDirection(RequestDirection go)
    {
        if(go.angle.Equals("up") && go.direction.Equals("FORWARD")) Forward();
        if (go.angle.Equals("up") && go.direction.Equals("RIGHT")) Right();
        if (go.angle.Equals("up") && go.direction.Equals("LEFT")) Left();
        if (go.angle.Equals("down") && go.direction.Equals("RIGHT")) Reverse_Right();
        if (go.angle.Equals("down") && go.direction.Equals("LEFT")) Reverse_Left();
        if (go.angle.Equals("down") && go.direction.Equals("BACKWARD")) Reverse();

        return go;
    }

    /// <summary>
    /// Forward
    /// </summary>
    private void Forward()
    {
        _MOTOR1.Forward();
        _MOTOR2.Forward();
    }

    /// <summary>
    /// Reverse
    /// </summary>
    private void Reverse()
    {
        _MOTOR1.Reverse();
        _MOTOR2.Reverse();
    }

    /// <summary>
    /// Stop
    /// </summary>
    public void Stop()
    {
        _MOTOR1.Stop();
        _MOTOR2.Stop();
    }

    /// <summary>
    /// Gets or sets the speed of the motor.
    /// Speed is a value from -1 to 1.
    /// 1 means maximum speed, signed value changes the direction.
    /// </summary>
    /// <param name="speed">selected speed </param>
    public void SetSpeed(double speed)
    {
        _MOTOR1.Speed = speed;
        _MOTOR2.Speed = speed;
    }


    /// <summary>
    /// Left
    /// </summary>
    private void Left()
    {
        _MOTOR1.Stop();
        _MOTOR2.Forward();
    }

    /// <summary>
    /// Reverse Left
    /// </summary>
    private void Reverse_Left()
    {
        _MOTOR1.Stop();
        _MOTOR2.Reverse();
    }

    /// <summary>
    /// Right
    /// </summary>
    private void Right()
    {
        _MOTOR1.Forward();
        _MOTOR2.Stop();
    }

    /// <summary>
    /// Reverse Right
    /// </summary>
    private void Reverse_Right()
    {
        _MOTOR1.Reverse();
        _MOTOR2.Stop();
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
            _disposeDrivers = _MOTOR1.Dispose(_disposeDrivers);
            _MOTOR2.Dispose(_disposeDrivers);
        }
        // Exit application
        Environment.Exit(-1);
    }


}


