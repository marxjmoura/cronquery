using CronQuery.Mvc.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using tests.Mvc.Jobs;

namespace tests.Mvc.Fakes
{
    public class StartupFake
    {
        public StartupFake(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCronQuery(Configuration.GetSection("CronQuery"));

            services.AddSingleton<ILoggerFactory, LoggerFactoryFake>();

            services.AddSingleton<JobWithError>();
            services.AddSingleton<JobSuccessful>();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            app.UseCronQuery()
                .Enqueue<JobSuccessful>()
                .Enqueue<JobWithError>()
                .StartWith(appLifetime);
        }
    }
}
