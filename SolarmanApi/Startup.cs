using System.Reflection.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Rest.Net.Interfaces;
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
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "SolarmanApi", Version = "v1"}); });

            ConfigureCustomServices(services);
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.Configure<SolarmanAuthenticationOptions>(Configuration.GetSection(nameof(SolarmanAuthenticationOptions)));
            services.Configure<SolarmanApiOptions>(Configuration.GetSection(nameof(SolarmanApiOptions)));

            services.AddSingleton<IAuthentication, SolarmanAuthentication>();
            services.AddSingleton<ISolarmanApi, SolarmanApiV1>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
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