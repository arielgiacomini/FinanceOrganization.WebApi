using Microsoft.AspNetCore;
using WebAPI;

namespace FinanceOrganization.WebAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}