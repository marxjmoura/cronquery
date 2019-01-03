using CronQuery.Mvc.Jobs;
using CronQuery.Mvc.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CronQuery.Mvc.DependencyInjection
{
    public static class CronQueryExtensions
    {
        public static void AddCronQuery(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.AddSingleton<JobRunner>();
            services.Configure<JobRunnerOptions>(configuration);
        }

        public static CronQuerySetup UseCronQuery(this IApplicationBuilder app)
        {
            var runner = app.ApplicationServices.GetRequiredService<JobRunner>();

            return new CronQuerySetup(runner);
        }
    }
}
