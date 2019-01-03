using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CronQuery.Cron;
using CronQuery.Mvc.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CronQuery.Mvc.Jobs
{
    internal class JobRunner : IDisposable
    {
        private JobRunnerOptions _options;
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;
        private ICollection<IDisposable> _timers;

        public JobRunner(IOptionsMonitor<JobRunnerOptions> options, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _options = options.CurrentValue;
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
            _timers = new List<IDisposable>();

            options.OnChange(Restart);
        }

        public ICollection<Type> JobTypes { get; private set; } = new List<Type>();

        public void Dispose()
        {
            foreach (var timer in _timers)
            {
                timer.Dispose();
            }
        }

        public void Enqueue<TJob>() where TJob : IJob
        {
            JobTypes.Add(typeof(TJob));
        }

        public void Start()
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(_options.Timezone ?? "UTC");

            foreach (var job in JobTypes)
            {
                var config = _options.Jobs.SingleOrDefault(entry => entry.Name == job.Name);

                if (config == null || !config.Running)
                {
                    continue;
                }

                var cron = new CronExpression(config.Cron);
                var timer = new JobInterval(cron, timezone, async () => await Do(job));

                _timers.Add(timer);

                timer.Run();
            }
        }

        private async Task Do(Type job)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobInstance = ((IJob)scope.ServiceProvider.GetRequiredService(job));

                try
                {
                    await jobInstance.RunAsync();
                }
                catch (Exception error)
                {
                    var logger = _loggerFactory.CreateLogger(job.FullName);
                    logger.LogCritical(error, $"Job {job.Name} failed during running.");
                }
                finally
                {
                    if (jobInstance is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        private void Restart(JobRunnerOptions options)
        {
            _options = options;

            foreach (var timer in _timers)
            {
                timer.Dispose();
            }

            _timers.Clear();

            if (options.Running)
            {
                Start();
            }
        }
    }
}
