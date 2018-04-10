#region

using System;
using System.Collections.Generic;
using PixelPubApi.Factory;
using PixelPubApi.Filters;
using PixelPubApi.Interfaces;
using PixelPubApi.Middleware;
using PixelPubApi.Models.Settings;
using PixelPubApi.Repository;
using PixelPubApi.Providers;
using PixelPubApi.MySQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

#endregion

namespace PixelPubApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env {get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Database connection
            var dbString = "server=208.97.173.174;database=winpixelpub;user=winpixelpub;password=wrathIncarnate";

            // Singleton Services
            services.AddSingleton<IRestClientFactory, RestClientFactory>();

            // Transient services
            services.AddTransient<WrathIncarnateContext>(s => new WrathIncarnateContext(dbString));
            services.AddTransient<IApiAccessProvider, ApiAccessProvider>();

            // config settings
            services.Configure<List<RestSource>>(Configuration.GetSection("RestEndpoints"));

            // Memcache
            services.AddMemoryCache();

            // dotnet core addition to return json or xml based on content type requested
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.Filters.Add(typeof(RequestCacheActionFilter));
                options.InputFormatters.Add(new XmlSerializerInputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Pixel Pub API", Version = "v1" });
            });     

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsEnvironment("Local") || env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pixel Pub API V1");
                });

            app.UseSwagger();
            app.UseMiddleware<ApiAuthMiddleWare>();
            //app.UseMiddleware<LoggingMiddleware>();

            app.UseMvc();
        }
    }
}
