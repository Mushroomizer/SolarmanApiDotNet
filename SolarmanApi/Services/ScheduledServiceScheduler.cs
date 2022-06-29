using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SolarmanApi.Interfaces;
using SolarmanApi.Options;

namespace SolarmanApi.Services
{
    public class ScheduledServiceScheduler : IHostedService
    {
        private readonly CronOptions _cronOptions;
        private readonly ILogger<ScheduledServiceScheduler> _logger;
        private readonly IEnumerable<IScheduledService> _scheduledServices;

        public ScheduledServiceScheduler(IEnumerable<IScheduledService> scheduledServices, IOptions<CronOptions> options, ILogger<ScheduledServiceScheduler> logger)
        {
            _scheduledServices = scheduledServices;
            _cronOptions = options.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleServicesAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping scheduler");
        }

        public Task ScheduleServicesAsync()
        {
            _logger.LogInformation("Scheduling services...");
            foreach (var scheduledService in _scheduledServices)
            {
                var serviceOptions = _cronOptions.FirstOrDefault(c => c.serviceName == scheduledService.GetType().Name);
                if (serviceOptions.runOnStart)
                {
                    _logger.LogInformation($"Executing {scheduledService.GetType().Name} - [{nameof(serviceOptions.runOnStart)} == true]");
                    scheduledService.Execute();
                }

                // Hangfire parses the cron string internally, dont need to do it here
                RecurringJob.AddOrUpdate($"{nameof(IScheduledService)}:{scheduledService.GetType().Name}", () => scheduledService.Execute(), serviceOptions.schedule);
            }

            return Task.CompletedTask;
        }
    }
}