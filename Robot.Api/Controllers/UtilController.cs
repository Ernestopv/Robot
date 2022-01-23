using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robot.Models;
using Robot.Services.Implementation;

namespace Robot.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        private readonly IUtilitesService _utilitiesService;
        private readonly Configuration _config;
        public UtilController(IUtilitesService utilitiesService, IOptions<Configuration> config)
        {
            _utilitiesService = utilitiesService;
            _config = config.Value;
        }


        [HttpGet("battery")]
        public async Task<IActionResult> GetBatteryResult()
        {
            var result = await _utilitiesService.GetBatteryStatus();

            return Ok(result);
        }

        [HttpGet("CameraStatus")]
        public IActionResult GetCameraStatus()
        {
            var response = new CameraResponse() { IsCameraOn = Helpers.JsonFile.ReadAppSetting("Camera")};
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
    }
}
