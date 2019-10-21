using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using React.AspNet;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;
using TestTask.Models;
using TestTask.Models.DataManager;
using TestTask.Models.Repository;

namespace TestTask {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();
            services.AddDbContext<VaccinationsContext>(o => o.UseSqlServer(Configuration["ConnectionString:DefaultConnection"]));
            services.AddScoped<IDataRepository<Patient>, PatientRepository>();
            services.AddScoped<IDataRepository<VaccinationVM>, VaccinationRepository>();
            services.AddScoped<IDataRepository<Vaccine>, VaccineRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vaccinations API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, IHostingEnvironment env) {
            app.UseReact(config => { });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("AppName", Assembly.GetExecutingAssembly().GetName().Name)
                .Enrich.WithProperty("AppVersion", Assembly.GetExecutingAssembly().GetName().Version)
                .CreateLogger();
            Log.Logger = logger;

            lifetime.ApplicationStarted.Register(() => Log.Write(LogEventLevel.Information, "Application started"));
            // не срабатывает при остановке приложения в visual studio
            lifetime.ApplicationStopped.Register(() => Log.Write(LogEventLevel.Information, "Application stopped"));

            // при такой остановке срабатывает (вызов graceful shutdown)
            //lifetime.StopApplication();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vaccinations API V1");
            });
        }
    }
}
