using Robot.Models;

namespace Robot.Services.Implementation;

public interface IMotorService
{
    RequestDirection SetDirection(RequestDirection go);
    public void SetSpeed(double speed);

    public void Stop();
    public void Dispose();
}

