using CronQuery.Mvc.Jobs;
using Microsoft.AspNetCore.Hosting;

namespace CronQuery.Mvc.DependencyInjection
{
    public class CronQuerySetup
    {
        private JobRunner runner;

        internal CronQuerySetup(JobRunner runner)
        {
            this.runner = runner;
        }

        public CronQuerySetup Enqueue<TJob>() where TJob : IJob
        {
            runner.Enqueue<TJob>();

            return this;
        }

        public void StartWith(IApplicationLifetime appLifetime)
        {
            appLifetime.ApplicationStarted.Register(runner.Start);
            appLifetime.ApplicationStopping.Register(runner.Dispose);
        }
    }
}
