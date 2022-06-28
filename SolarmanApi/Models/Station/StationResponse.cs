using System.Collections.Generic;

namespace SolarmanApi.Models
{
    public class StationResponse : BaseResponse
    {
        public int total { get; set; }
        public List<StationData> stationList { get; set; }
    }
}