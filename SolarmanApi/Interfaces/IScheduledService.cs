using System.Threading.Tasks;

namespace SolarmanApi.Interfaces
{
    // Any ISheduledService that exists in the assembly will be added to the service schedule automatically
    public interface IScheduledService
    {
        public Task Execute();
    }
}