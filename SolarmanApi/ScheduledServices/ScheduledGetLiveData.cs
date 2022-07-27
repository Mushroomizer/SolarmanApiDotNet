using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            _logger.LogInformation($"Realtime Data: {JsonConvert.SerializeObject(data)}");
            var lastUpdatedDatetime = DateTimeOffset.FromUnixTimeSeconds(data.lastUpdateTime).DateTime;
            _logger.LogInformation($"Last updated at: {lastUpdatedDatetime.ToString()}");
            // data.gridPower does not represent purchased power, so using wirePower instead
            if (data.wirePower == 0)
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