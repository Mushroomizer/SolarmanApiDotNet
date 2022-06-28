using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarmanApi.Interfaces;
using SolarmanApi.Services;

namespace SolarmanApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SolarmanController : ControllerBase
    {
        private readonly ISolarmanApi _solarmanApi;
        private readonly ILogger<SolarmanController> _logger;

        public SolarmanController(ISolarmanApi solarmanApi, ILogger<SolarmanController> logger)
        {
            _solarmanApi = solarmanApi;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentLiveData()
        {
            try
            {
                return Ok(await _solarmanApi.GetRealtimeData());
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}\n{e.StackTrace}");
            }
        }
        
    }
}