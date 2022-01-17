using System.Device.Gpio;
using System.Device.Pwm.Drivers;

namespace Robot.L298N;
/// <summary>
/// L298NDriver
/// </summary>
public class L298NDriver
{
    private const int DefaultPwmFrequency = 50;
    private double _speed = 0.0;
    private GpioController _controler;
    private readonly SoftwarePwmChannel _pwm;
    private int _pin0, _pin1;


    /// <summary>
    /// Gets or sets the speed of the motor.
    /// Speed is a value from -1 to 1.
    /// 1 means maximum speed, signed value changes the direction.
    /// </summary>
    public double Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            double val = Math.Clamp(value, -1.0, 1.0);
            _pwm.DutyCycle = Math.Abs(val);
            _speed = val;
        }
    }

    /// <summary>
    /// L298N driver 
    /// </summary>
    /// <param name="speedControlPin"></param>
    /// <param name="pi0"></param>
    /// <param name="pin1"></param>
    /// <param name="controller"></param>
    public L298NDriver(int speedControlPin, int pi0, int pin1, GpioController controller)
    {
        _controler = controller;
        _pin0 = pi0;
        _pin1 = pin1;
        _pwm = SetupPWMChannel(speedControlPin, DefaultPwmFrequency);
        SetupMotor(_pin0, _pin1);
        _pwm.Start();

    }

    /// <summary>
    /// Forward 
    /// </summary>
    public void Forward()
    {
        if (Speed == 0.0) Speed = 0.2;
        _controler.Write(_pin0, PinValue.High);
        _controler.Write(_pin1, PinValue.Low);
    }

    /// <summary>
    /// Reverse
    /// </summary>
    public void Reverse()
    {
        if (Speed == 0.0) Speed = 0.2;
        _controler.Write(_pin0, PinValue.Low);
        _controler.Write(_pin1, PinValue.High);
    }

    /// <summary>
    /// Stop
    /// </summary>
    public void Stop()
    {
        _controler.Write(_pin0, PinValue.Low);
        _controler.Write(_pin1, PinValue.Low);
    }

    /// <summary>
    /// Setup Motor Configuration
    /// </summary>
    /// <param name="pwmChannel"></param>
    /// <param name="pin0"></param>
    /// <param name="pin1"></param>
    /// <param name="controller"></param>
    private void SetupMotor(int pin0, int pin1)
    {
        _controler.OpenPin(pin0, PinMode.Output);
        _controler.Write(pin0, PinValue.Low);
        _controler.OpenPin(pin1, PinMode.Output);
        _controler.Write(pin1, PinValue.Low);
    }

    /// <summary>
    /// Set PWM Channel Configuration 
    /// </summary>
    /// <param name="speedControlPin"></param>
    /// <param name="DefaultPwmFrequency"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    private SoftwarePwmChannel SetupPWMChannel(int speedControlPin, int DefaultPwmFrequency)
    {
        var softwareChannel = new SoftwarePwmChannel(speedControlPin, DefaultPwmFrequency, 0.0, controller: _controler);
        return softwareChannel;
    }

    /// <summary>
    /// Dispose Controller
    /// </summary>
    public bool Dispose(bool disposeController)
    {
        if (disposeController == false)
        {
            _pwm.Dispose();
            _controler.Dispose();
        }

        return true;

    }
}
