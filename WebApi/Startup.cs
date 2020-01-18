using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public delegate IMainProcessingRepository RepositoryServiceResolver(string key);
        public delegate IAnalysisResultProvider AnalysisResultServiceResolver(string key);
        public delegate IPatientProvider PatientServiceResolver(string key);
        public delegate IDiagnosisProvider DiagnosisServiceResolver(string key);
        public delegate IRuleProvider RuleServiceResolver(string key);
        public delegate IProcessedResultProvider ProcessedResultServiceResolver(string key);
        public delegate IReportGenerator ReportGeneratorResolver(string key);

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

            services.AddTransient<RepositoryServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Main":
                        return serviceProvider.GetService<MainRepositoryWrapper>();
                    case "Learning":
                        return serviceProvider.GetService<LearningRepositoryWrapper>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddTransient<AnalysisResultDbProvider>();
            services.AddTransient<AnalysisResultLearningDbProvider>();

            services.AddTransient<AnalysisResultServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Main":
                        return serviceProvider.GetService<AnalysisResultDbProvider>();
                    case "Learning":
                        return serviceProvider.GetService<AnalysisResultLearningDbProvider>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddTransient<PatientDbProvider>();
            services.AddTransient<PatientLearningDbProvider>();

            services.AddTransient<PatientServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Main":
                        return serviceProvider.GetService<PatientDbProvider>();
                    case "Learning":
                        return serviceProvider.GetService<PatientLearningDbProvider>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddTransient<DiagnosisDbProvider>();
            services.AddTransient<DiagnosisLearningDbProvider>();

            services.AddTransient<DiagnosisServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Main":
                        return serviceProvider.GetService<DiagnosisDbProvider>();
                    case "Learning":
                        return serviceProvider.GetService<DiagnosisLearningDbProvider>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddTransient<RuleDbProvider>();
            services.AddTransient<RuleLearningDbProvider>();

            services.AddTransient<RuleServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Main":
                        return serviceProvider.GetService<RuleDbProvider>();
                    case "Learning":
                        return serviceProvider.GetService<RuleLearningDbProvider>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddTransient<TxtReportGenerator>();
            services.AddTransient<HtmlReportGenerator>();
            
            services.AddTransient<ReportGeneratorResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Txt":
                        return serviceProvider.GetService<TxtReportGenerator>();
                    case "Html":
                        return serviceProvider.GetService<HtmlReportGenerator>();
                    default:
                        throw new KeyNotFoundException();
                }
            });
            
            services.AddTransient<LearningProcessedResultDbProvider>();

            services.AddTransient<ProcessedResultServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Learning":
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
