using Application;
using Domain.Options;
using Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        const string DEFAULT_LOG_DIRECTORY = "C:/Logs/LogFinanceOrganization_.log";
        private readonly Uri URL_ARIELGIACOMINI = new("http://teste.com.br");

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            var logPathAppSettings = Configuration.GetSection("Log:Path").Value;
            var filePath = logPathAppSettings is null ? DEFAULT_LOG_DIRECTORY : logPathAppSettings;

            services.Configure<BillToPayOptions>(options =>
            Configuration.GetSection("BillToPayOptions").Bind(options));

            services.AddHostedServices();
            services.AddApplication();
            services.AddInfrastructure();

            services.AddSingleton<Serilog.ILogger, Serilog.Core.Logger>(x =>
            {
                var logger = new LoggerConfiguration()
                .WriteTo.File(filePath!, rollingInterval: RollingInterval.Day)
                .CreateLogger();

                return logger;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1.6",
                    Title = "Resposável por criar organizar financeiramente",
                    Description = $"Já está preparado para criar as contas a pagar - Ariel Giacomini - última versão gerada em [{DateTime.Now}]",
                    TermsOfService = URL_ARIELGIACOMINI,
                    Contact = new OpenApiContact
                    {
                        Name = "Ariel Giacomini da Silva",
                        Email = "arieltecnologia@outlook.com",
                        Url = URL_ARIELGIACOMINI
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Não há licença",
                        Url = URL_ARIELGIACOMINI
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "SGM");
                c.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}