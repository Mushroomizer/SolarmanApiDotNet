using System.Reflection;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Rest.Net.Interfaces;
using Serilog;
using SolarmanApi.Extensions;
using SolarmanApi.Interfaces;
using SolarmanApi.Options;
using SolarmanApi.Services;

namespace SolarmanApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "SolarmanApi", Version = "v1"}); });

            ConfigureCustomServices(services);
            ConfigureHangfire(services);
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.Configure<SolarmanAuthenticationOptions>(Configuration.GetSection(nameof(SolarmanAuthenticationOptions)));
            services.Configure<SolarmanApiOptions>(Configuration.GetSection(nameof(SolarmanApiOptions)));

            services.AddSingleton<IAuthentication, SolarmanAuthentication>();
            services.AddSingleton<ISolarmanApi, SolarmanApiV1>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public void ConfigureHangfire(IServiceCollection services)
        {
            services.AddHangfire(c =>
            {
                c.UseSimpleAssemblyNameTypeSerializer();
                c.UseRecommendedSerializerSettings();
                c.UseMemoryStorage();
            });

            services.AddHangfireServer();
            StartScheduledServices(services);
        }

        public void StartScheduledServices(IServiceCollection services)
        {
            services.Configure<CronOptions>(Configuration.GetSection(nameof(CronOptions)));
            services.RegisterAllTypes<IScheduledService>(new[] {Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()}!, ServiceLifetime.Singleton);
            services.AddHostedService<ScheduledServiceScheduler>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SolarmanApi v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}