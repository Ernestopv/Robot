using Microsoft.AspNetCore.Mvc;
using Robot.Infrastructure.Settings;
using Robot.Models;
using Robot.Services.Interfaces;

namespace Robot.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        private readonly IUtilitesService _utilitiesService;

        private readonly AppSettings? _appSettings;
        public UtilController(IUtilitesService utilitiesService,IConfiguration configuration)
        {
            _appSettings = configuration.Get<AppSettings>();
            _utilitiesService = utilitiesService;
        }


        [HttpGet("battery")]
        public async Task<IActionResult> GetBatteryResult()
        {
            var result = await _utilitiesService.GetBatteryStatus();

            return Ok(result);
        }

        [HttpGet("ConfigStatus")]
        public IActionResult GetConfigStatus()
        {
            var response = new ConfigResponse()
            {
                IsCameraOn = _appSettings!.Camera,
                Speed = _appSettings.Speed,
            };

            return Ok(response);
        }

        [HttpGet("CameraOn")]
        public async Task<IActionResult> SetCameraON()
        {
            Helpers.JsonFile.AddOrUpdateAppSetting("Camera", true);
             var result = await _utilitiesService.TurnOnCamera();

            return Ok(result);
        }

        [HttpGet("CameraOff")]
        public async Task<IActionResult> SetCameraOFF()
        {
            var result = await _utilitiesService.TurnOffCamera();
            Helpers.JsonFile.AddOrUpdateAppSetting("Camera", false);

            return Ok(result);
        }

        [HttpGet("Ip")]
        public async Task<IActionResult> GetIpAddress()
        {
            var result = await _utilitiesService.GetIP();
            return Ok(result);
        }
    }
}
