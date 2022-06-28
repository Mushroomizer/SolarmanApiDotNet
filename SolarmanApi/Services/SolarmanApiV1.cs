using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rest.Net;
using Rest.Net.Interfaces;
using SolarmanApi.Interfaces;
using SolarmanApi.Models;
using SolarmanApi.Options;

namespace SolarmanApi.Services
{
    public class SolarmanApiV1 : ISolarmanApi
    {
        private readonly IAuthentication _authentication;
        private readonly ILogger<SolarmanApiV1> _logger;
        private readonly SolarmanApiOptions _solarmanApiOptions;
    
        public SolarmanApiV1(IAuthentication authentication,IOptions<SolarmanApiOptions> solarmanApiOptions, ILogger<SolarmanApiV1> logger)
        {
            _authentication = authentication;
            _logger = logger;
            _solarmanApiOptions = solarmanApiOptions.Value;
        }

        private IRestClient CreateRestClient()
        {
            var restClient = new RestClient(_solarmanApiOptions.BaseUrl)
            {
                Authentication = _authentication
            };
            
            return restClient;
        }


        private async Task<List<StationData>> GetStationDataAsync()
        {
            try
            {
                _logger.LogInformation("Getting station list");
                var restClient = CreateRestClient();
                restClient.AddParameter("language","en");
                var response = await restClient.PostAsync("/station/v1.0/list", new
                {
                    page = 1,
                    size = 20
                },true);
                var parsedData = JsonConvert.DeserializeObject<StationResponse>(response.RawData.ToString());
                _logger.LogInformation($"Station list response: {JsonConvert.SerializeObject(parsedData)}");
            
                return parsedData.stationList.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Could not get station list");
                throw;
            }
        }
        
        
        public async Task<RealtimeDataResponse> GetRealtimeData()
        {
            try
            {
                var restClient = CreateRestClient();
                restClient.AddParameter("language","en");
            
                var stationList = await GetStationDataAsync();

                _logger.LogInformation("Getting realtime data");
                var response = await restClient.PostAsync<RealtimeDataResponse>("/station/v1.0/realTime", new
                {
                    stationId = stationList[0].id
                }, true);
                _logger.LogInformation($"Realtime data response: {JsonConvert.SerializeObject(response)}");
                return response.Data;
            }
            catch (Exception e)
            {
               _logger.LogError(e,"Could not get realtime data");
               throw;
            }
        }
    }
}