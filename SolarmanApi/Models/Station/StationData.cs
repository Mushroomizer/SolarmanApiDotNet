namespace SolarmanApi.Models
{
    public class StationData
    {
        public int id { get; set; }
        public string name { get; set; }
        public double locationLat { get; set; }
        public double locationLng { get; set; }
        public string locationAddress { get; set; }
        public int regionNationId { get; set; }
        public int regionLevel1 { get; set; }
        public int regionLevel2 { get; set; }
        public int regionLevel3 { get; set; }
        public int regionLevel4 { get; set; }
        public int regionLevel5 { get; set; }
        public string regionTimezone { get; set; }
        public string type { get; set; }
        public string gridInterconnectionType { get; set; }
        public double installedCapacity { get; set; }
        public double startOperatingTime { get; set; }
        public string stationImage { get; set; }
        public double createdDate { get; set; }
        public double batterySoc { get; set; }
        public string networkStatus { get; set; }
        public double generationPower { get; set; }
        public double lastUpdateTime { get; set; }
    }
}