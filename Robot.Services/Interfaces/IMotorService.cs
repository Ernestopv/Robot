using Robot.Models;

namespace Robot.Services.Interfaces;

public interface IMotorService
{
    RequestDirection SetDirection(RequestDirection go);
    public void SetSpeed(double speed);

    public void Stop();
    public void Dispose();
}

