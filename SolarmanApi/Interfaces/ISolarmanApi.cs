using System.Threading.Tasks;
using SolarmanApi.Models;

namespace SolarmanApi.Interfaces
{
  public interface ISolarmanApi
  {
      public Task<RealtimeDataResponse> GetRealtimeData();
  }  
}


