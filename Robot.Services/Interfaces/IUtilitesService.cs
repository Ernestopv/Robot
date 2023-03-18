using Robot.Models;

namespace Robot.Services.Interfaces;

public interface IUtilitesService
{
    Task<IpResponse> GetIP();
    Task<BatteryResponse> GetBatteryStatus();
    Task<ConfigResponse> TurnOnOffCamera(bool isOn);
}

