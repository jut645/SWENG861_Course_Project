using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FlightPrices.Skyscanner.WebAPI.Clients;
using FlightPrices.Skyscanner.WebAPI.Responses;
using FlightPrices.Skyscanner.WebAPI.Clients.Contracts;
using FlightPrices.Skyscanner.WebAPI.Models;
using System.Net.Http;

namespace FlightPrices.Skyscanner.WebAPI
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

            services.AddHttpClient();

            var skyscannerApiKey = Configuration.GetSection("SkyscannerApiKey").Value;
            services.AddSingleton(typeof(ApiKey), new ApiKey(skyscannerApiKey));

            var skyscannerBaseUrl = Configuration.GetSection("SkyscannerBaseUrl").Value;
            services.AddSingleton(typeof(SkyscannerBaseUrl), new SkyscannerBaseUrl(skyscannerBaseUrl));

            services.AddTransient(typeof(ISkyscannerClient), typeof(TravelAdvisorClient));
            services.AddDbContext<FlightPricesContext>();

            services.AddDbContext<FlightPricesContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
