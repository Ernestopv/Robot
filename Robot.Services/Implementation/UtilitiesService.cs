using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Robot.Infrastructure.Settings;
using Robot.Linux;
using Robot.Models;
using Robot.Services.Helpers;
using Robot.Services.Interfaces;

namespace Robot.Services.Implementation
{
    public class UtilitiesService : IUtilitesService
    {
        private readonly ILogger<UtilitiesService> _logger;

        private readonly AppSettings _appSettings;
        public UtilitiesService(ILogger<UtilitiesService> logger, IOptionsSnapshot<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }
        public async Task<BatteryResponse> GetBatteryStatus()
        {
            switch (_appSettings!.IsUpsAvailable)
            {
                case true:
                {
                    await BatteryCommandAsync();

                    var lines = await File.ReadAllLinesAsync("battery.txt");
                    var chargeData = lines[1];
                    var percentageData = lines[3].Split(":");
                    var percentage = percentageData[1].Replace("%", "").Trim();
                    var batteryResponse = new BatteryResponse
                    {
                        Charging = !chargeData.Contains('-'),
                        Percentage = (int)Convert.ToSingle(percentage)
                    };

                    return batteryResponse;
                }
                default:
                    return new BatteryResponse()
                    {
                        Charging = false,
                        Percentage = 50,
                    };
            }
        }


        public async Task<IpResponse> GetIP()
        {
            if (_appSettings!.IsCameraAvailable)
            {
                await IpCommandAsync();
                var ipText = await File.ReadAllTextAsync("IpAddress.txt");
                return new IpResponse { Ip = ipText.Trim() };
            }
            return new IpResponse { Ip = "localhost" };
        }

        public async Task<ConfigResponse> TurnOnOffCamera( bool isOn)
        {
            var configResponse = new ConfigResponse();
            JsonFile.AddOrUpdateAppSetting("Camera", isOn);
            if (_appSettings!.IsCameraAvailable)
            {
                if (isOn)
                {
                    var cameraStatus = await TurnON();
                    configResponse.IsCameraOn = cameraStatus;
                    return configResponse;
                }
                else
                {
                    var cameraStatus = await TurnOff();
                    configResponse.IsCameraOn = cameraStatus;
                    return configResponse;

                }
            
            }

            configResponse.IsCameraOn = isOn;
            return configResponse;
        }


        private async Task<int> BatteryCommandAsync()
        {
            return await "sudo python3  /home/ubuntu/UPS_HAT/INA219.py > /home/ubuntu/Robot/battery.txt".Bash(_logger);
        }

        private async Task<bool> TurnON()
        {
            await "sudo sh /home/ubuntu/mjpg-streamer/Start_mjpg_Streamer.sh".Sh(_logger);
            return true;
        }

        private async Task<bool> TurnOff()
        {
            await "sudo killall  mjpg_streamer".Bash(_logger);
            return false;
        }

        private async Task<int> IpCommandAsync()
        {
            return await "hostname -I | awk '{print $1}' > /home/ubuntu/Robot/IpAddress.txt".Bash(_logger);
        }

    }
}
