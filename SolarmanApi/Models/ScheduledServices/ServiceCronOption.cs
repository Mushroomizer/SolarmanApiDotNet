namespace SolarmanApi.Models.ScheduledServices
{
    public class ServiceCronOption
    {
        public string serviceName { get; set; }
        public string schedule { get; set; } = "*/1 * * * *";
        public bool runOnStart { get; set; } = false;
    }
}