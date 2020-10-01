using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebsocketGracefulShutdown22
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseShutdownTimeout(TimeSpan.FromSeconds(30))
                .UseStartup<Startup>();
    }
}
