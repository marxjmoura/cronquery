using System;
using System.IO;
using CronQuery.Mvc.Jobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace tests.Mvc.Fakes
{
    public class ServerFake : TestServer
    {
        public ServerFake() : base(WebHostBuilder()) { }

        public LoggerFake Logger => Host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(string.Empty) as LoggerFake;
        public TJob Job<TJob>() where TJob : IJob => Host.Services.GetRequiredService<TJob>();

        private static IWebHostBuilder WebHostBuilder() => new WebHostBuilder()
            .UseStartup<StartupFake>()
            .UseEnvironment("Testing")
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                var build = Path.Combine("bin", "Debug", "netcoreapp2.2");
                var root = AppContext.BaseDirectory.Replace(build, string.Empty);
                var appsettings = Path.Combine(root, "Mvc", "appsettings.json");

                config.AddJsonFile(appsettings, optional: false, reloadOnChange: true);
            });
    }
}
