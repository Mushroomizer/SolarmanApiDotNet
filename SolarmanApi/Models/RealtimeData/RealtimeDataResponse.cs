namespace SolarmanApi.Models
{
    public class RealtimeDataResponse : BaseResponse
    {
        public double generationPower { get; set; }
        public double usePower { get; set; }
        public double gridPower { get; set; }
        public double purchasePower { get; set; }
        public double wirePower { get; set; }
        public double chargePower { get; set; }
        public double dischargePower { get; set; }
        public double batteryPower { get; set; }
        public double batterySoc { get; set; }
        public double irradiateIntensity { get; set; }
        public long lastUpdateTime { get; set; }
    }
}