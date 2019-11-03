using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Implementations;
using WebApi.Interfaces;
using WebApi.POCO;

#pragma warning disable 1591

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; set; }
        private string _appPath;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Environment = environment;
            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1"
                });

                c.IncludeXmlComments(_appPath);
            });
            services.AddScoped<IDiagnosisProvider, DiagnosisProvider>();
            services.AddScoped<IMainProcessingRepository, MainProcessingRepository>();
            services.Configure<DatabaseOptions>(Configuration.GetSection("ProcessingDb:DefaultConnection"));
            services.Configure<Config>(Configuration.GetSection("Config"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            _appPath = System.IO.Path.Combine(env.ContentRootPath, "WebApi.xml");
        }
    }
}
