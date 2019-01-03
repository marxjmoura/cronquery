using CronQuery.Mvc.DependencyInjection;
using example.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddCronQuery(Configuration.GetSection("CronQuery"));

            services.AddTransient<MyFirstJob>();
            services.AddTransient<MySecondJob>();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            app.UseMvc();

            app.UseCronQuery()
                .Enqueue<MyFirstJob>()
                .Enqueue<MySecondJob>()
                .StartWith(appLifetime);
        }
    }
}
