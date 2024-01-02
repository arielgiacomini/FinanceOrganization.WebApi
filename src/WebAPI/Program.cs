using Microsoft.AspNetCore;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebAPI;

namespace FinanceOrganization.WebAPI
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder<Program, Startup>(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }

        public static IHostBuilder CreateHostBuilder<TProgram, TStartup>(string[] args) where TProgram : class where TStartup : class
        {
            _ = Activity.DefaultIdFormat - ActivityIdFormat.W3C;
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(delegate (IWebHostBuilder webBuilder)
            {
                webBuilder.ConfigureAppConfiguration(delegate (WebHostBuilderContext builderContext, IConfigurationBuilder builder)
                {
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    if (!builderContext.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets<TProgram>();
                    }
                    webBuilder.UseStartup<TStartup>();
                });
            });
        }
    }
}