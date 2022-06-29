using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarmanApi.Interfaces;

namespace SolarmanApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SolarmanController : ControllerBase
    {
        private readonly ILogger<SolarmanController> _logger;
        private readonly ISolarmanApi _solarmanApi;

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
                return Ok(await _solarmanApi.GetRealtimeDataAsync());
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}