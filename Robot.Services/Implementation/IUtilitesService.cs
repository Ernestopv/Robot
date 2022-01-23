using Robot.Models;

namespace Robot.Services.Implementation;

public interface IUtilitesService
{
    string GetIP();
    Task<BatteryResponse> GetBatteryStatus();

    Task<CameraResponse> TurnOffCamera();

    Task<CameraResponse> TurnOnCamera();
}

