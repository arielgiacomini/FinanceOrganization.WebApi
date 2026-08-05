using Application;
using Domain.Options;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                    policy
                        .SetIsOriginAllowed(_ => true)  // ← aceita qualquer origem
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });

            var logPathAppSettings = Configuration.GetSection("Log:Path").Value;
            var filePath = logPathAppSettings is null ? DEFAULT_LOG_DIRECTORY : logPathAppSettings;

            services.Configure<BillToPayOptions>(options =>
            Configuration.GetSection("BillToPayOptions").Bind(options));

            services.Configure<CategoryOptions>(options =>
            Configuration.GetSection("CategoryOptions").Bind(options));

            services.Configure<CashReceivableOptions>(options =>
            Configuration.GetSection("CashReceivableOptions").Bind(options));

            services.Configure<GenericBackgroundServiceOptions>(options =>
             Configuration.GetSection("GenericBackgroundServiceOptions").Bind(options));

            services.Configure<JwtOptions>(options =>
             Configuration.GetSection("Jwt").Bind(options));

            services.Configure<AuthClientOptions>(options =>
             Configuration.GetSection("AuthClient").Bind(options));

            var jwtOptions = new JwtOptions();
            Configuration.GetSection("Jwt").Bind(jwtOptions);

            var authClientOptions = new AuthClientOptions();
            Configuration.GetSection("AuthClient").Bind(authClientOptions);

            if (string.IsNullOrWhiteSpace(jwtOptions.Secret))
            {
                throw new InvalidOperationException("Configuração 'Jwt:Secret' não foi definida. Configure via 'dotnet user-secrets' (dev) ou variável de ambiente 'Jwt__Secret' (produção).");
            }

            if (string.IsNullOrWhiteSpace(authClientOptions.ClientId) || string.IsNullOrWhiteSpace(authClientOptions.ClientSecret))
            {
                throw new InvalidOperationException("Configuração 'AuthClient:ClientId'/'AuthClient:ClientSecret' não foi definida. Configure via 'dotnet user-secrets' (dev) ou variáveis de ambiente 'AuthClient__ClientId'/'AuthClient__ClientSecret' (produção).");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            services.AddAuthorization();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(new AuthorizeFilter());
            });

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

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "2.4",
                    Title = "Controle Financeiro Residencial",
                    Description = $"Último restart da aplicação: [{DateTime.Now}] - Rotas de Dashboard",
                    TermsOfService = URL_ARIELGIACOMINI,
                    Contact = new OpenApiContact
                    {
                        Name = "Ariel Giacomini da Silva",
                        Email = "contato@arielgiacomini.com.br",
                        Url = URL_ARIELGIACOMINI
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Não há licença",
                        Url = URL_ARIELGIACOMINI
                    }
                });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Informe o token JWT obtido em 'v1/auth/token'."
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
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

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

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