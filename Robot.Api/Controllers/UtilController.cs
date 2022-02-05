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
        public UtilController(IUtilitesService utilitiesService)
        {
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
            var response = Helpers.JsonFile.ReadAppSetting("Camera", "Speed");

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
