using Newtonsoft.Json;

namespace SolarmanApi.Models
{
    public class BaseResponse
    {
        public int code { get; set; }
        public string msg { get; set; }
        public bool success { get; set; }
        public string requestId { get; set; }
    }
}

