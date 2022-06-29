using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolarmanApi.Interfaces;

namespace SolarmanApi.ScheduledServices
{
    public class ScheduledGetLiveData : IScheduledService
    {
        private readonly ILogger<ScheduledGetLiveData> _logger;
        private readonly ISolarmanApi _solarmanApi;

        public ScheduledGetLiveData(ILogger<ScheduledGetLiveData> logger, ISolarmanApi solarmanApi)
        {
            _logger = logger;
            _solarmanApi = solarmanApi;
        }

        public async Task Execute()
        {
            var data = await _solarmanApi.GetRealtimeDataAsync();
            _logger.LogInformation($"Battery level: {data.batterySoc.ToString()}");
            _logger.LogInformation($"Generation: {data.generationPower.ToString()}");
            _logger.LogInformation($"Grid power: {data.gridPower.ToString()}");

            if (data.gridPower == 0)
            {
                _logger.LogWarning("Grid is offline!!!");
            }

            if (data.batteryPower > 0)
            {
                _logger.LogWarning("Battery is discharging!!!");
            }
        }
    }
}