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

        [HttpPost("Camera")]
        public async Task<IActionResult> SetCameraOnOff(bool isOn)
        {
           
            var result = await _utilitiesService.TurnOnOffCamera(isOn);
            
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
