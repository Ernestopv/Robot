using Robot.Models;

namespace Robot.Services.Implementation;

public interface IUtilitesService
{
    Task<IpResponse> GetIP();
    Task<BatteryResponse> GetBatteryStatus();

    Task<ConfigResponse> TurnOffCamera();

    Task<ConfigResponse> TurnOnCamera();
}

