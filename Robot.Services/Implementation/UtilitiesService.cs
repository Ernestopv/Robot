using Microsoft.Extensions.Logging;
using Robot.Linux;
using Robot.Models;

namespace Robot.Services.Implementation
{
    public class UtilitiesService : IUtilitesService
    {
        private readonly ILogger<UtilitiesService> _logger;
        public UtilitiesService(ILogger<UtilitiesService> logger)
        {
            _logger = logger;
        }
        public async Task<BatteryResponse> GetBatteryStatus()
        {
            await BatteryCommandAsync();

            string[] lines = await File.ReadAllLinesAsync("battery.txt");
            var chargeData = lines[1];
            var percentageData = lines[3].Split(":");
            string percentage = percentageData[1].Replace("%", "").Trim();
            var battery = new BatteryResponse();
            if (!chargeData.Contains('-')) battery.Charging = true;
            else battery.Charging = false;
            battery.Percentage = (int)Convert.ToSingle(percentage);

            return battery;
        }


        public string GetIP()
        {
            return "";
        }

        public async Task<CameraResponse> TurnOffCamera()
        {
            return new CameraResponse()
            {
                IsCameraOn = await TurnOFF()
            };
        }

        public async Task<CameraResponse> TurnOnCamera()
        {
            return new CameraResponse()
            {
                IsCameraOn = await TurnON()
            };
        }


        private async Task<int> BatteryCommandAsync()
        {
            return await ShellHelper.Bash(" sudo python3  /home/ubuntu/UPS_HAT/INA219.py > /home/ubuntu/Robot/battery.txt", _logger);
        }

        private async Task<bool> TurnON()
        {
            await ShellHelper.Sh(" sudo sh /home/ubuntu/mjpg-streamer/Start_mjpg_Streamer.sh", _logger);
            return true;
        }

        private async Task<bool> TurnOFF()
        {
            await ShellHelper.Bash("sudo killall  mjpg_streamer", _logger);
            return false;
        }


    }
}
