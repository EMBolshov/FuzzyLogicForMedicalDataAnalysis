using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POCO.Domain;
using Repository;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Implementations.Helpers;
using WebApi.Implementations.Learning;
using WebApi.Implementations.MainProcessing;
using WebApi.Interfaces.Helpers;
using WebApi.Interfaces.Learning;
using WebApi.Interfaces.MainProcessing;
using WebApi.POCO;

#pragma warning disable 1591

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; set; }

        public delegate IService ServiceResolver(string key);
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
            
            services.Configure<Config>(Configuration.GetSection("Config"));
            services.AddSingleton<IFileParser, FileParser>();
            services.AddSingleton<INamingMapper, AnalysisAndTestsNamingMapper>();
            services.AddTransient<ILearningProcessor, LearningProcessor>();
            services.AddSingleton<IEntitiesToCreateDtoMapper, EntitiesToCreateDtoMapper>();

            services.AddTransient<MainRepositoryWrapper>();
            services.AddTransient<LearningRepositoryWrapper>();
            services.AddTransient<AnalysisResultDbProvider>();
            services.AddTransient<AnalysisResultLearningDbProvider>();
            services.AddTransient<PatientDbProvider>();
            services.AddTransient<PatientLearningDbProvider>();
            services.AddTransient<RuleDbProvider>();
            services.AddTransient<RuleLearningDbProvider>();
            services.AddTransient<DiagnosisDbProvider>();
            services.AddTransient<DiagnosisLearningDbProvider>();
            services.AddTransient<TxtReportGenerator>();
            services.AddTransient<HtmlReportGenerator>();
            services.AddTransient<LearningProcessedResultDbProvider>();

            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "MainRepo":
                        return serviceProvider.GetService<MainRepositoryWrapper>();

                    case "LearningRepo":
                        return serviceProvider.GetService<LearningRepositoryWrapper>();

                    case "AnalysisResultMain":
                        return serviceProvider.GetService<AnalysisResultDbProvider>();

                    case "AnalysisResultLearning":
                        return serviceProvider.GetService<AnalysisResultLearningDbProvider>();

                    case "PatientMain":
                        return serviceProvider.GetService<PatientDbProvider>();

                    case "PatientLearning":
                        return serviceProvider.GetService<PatientLearningDbProvider>();

                    case "DiagnosisMain":
                        return serviceProvider.GetService<DiagnosisDbProvider>();

                    case "DiagnosisLearning":
                        return serviceProvider.GetService<DiagnosisLearningDbProvider>();

                    case "RuleMain":
                        return serviceProvider.GetService<RuleDbProvider>();

                    case "RuleLearning":
                        return serviceProvider.GetService<RuleLearningDbProvider>();

                    case "Txt":
                        return serviceProvider.GetService<TxtReportGenerator>();

                    case "Html":
                        return serviceProvider.GetService<HtmlReportGenerator>();

                    case "ProcessedResultLearning":
                        return serviceProvider.GetService<LearningProcessedResultDbProvider>();

                    default:
                        throw new KeyNotFoundException();
                }
            });
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
