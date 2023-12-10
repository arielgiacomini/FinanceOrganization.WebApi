using Application;
using Domain.Options;
using Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

namespace WebAPI
{
    public class Startup
    {
        const string DEFAULT_LOG_DIRECTORY = "C:/Logs/SGM.WebApi";
        private const string URL_ARIELGIACOMINI = "http://arielgiacomini.com.br";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            var filePath = Configuration.GetSection("Log:Path") is null ? DEFAULT_LOG_DIRECTORY : Configuration.GetSection("Log:Path").Value;

            services.Configure<WalletToPayOptions>(options =>
            Configuration.GetSection("WalletToPayOptions").Bind(options));

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
                    Version = "7.6",
                    Title = "Exposição do banco de dados das aplicações do SGM",
                    Description = "Nesta versão temos logs na forma de mao de obra controller - gabriel araújo",
                    TermsOfService = new Uri(URL_ARIELGIACOMINI),
                    Contact = new OpenApiContact
                    {
                        Name = "Ariel Giacomini da Silva",
                        Email = "arieltecnologia@outlook.com",
                        Url = new Uri(URL_ARIELGIACOMINI)
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Não há licença",
                        Url = new Uri(URL_ARIELGIACOMINI)
                    }
                });
            });
        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
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