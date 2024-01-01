using Microsoft.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using WebAPI;

namespace FinanceOrganization.WebAPI
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .UseEnvironment("")
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    };
}