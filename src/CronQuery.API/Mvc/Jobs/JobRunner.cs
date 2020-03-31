/*
 * MIT License
 *
 * Copyright (c) 2018 Marx J. Moura
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

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
    public sealed class JobRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ICollection<IDisposable> _timers;
        private readonly ICollection<Type> _jobs;

        private JobRunnerOptions _options;

        public JobRunner(IOptionsMonitor<JobRunnerOptions> options, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _timers = new List<IDisposable>();
            _jobs = new List<Type>();

            options.OnChange(Restart);
        }

        public IEnumerable<Type> Jobs => _jobs;

        public void Enqueue<TJob>() where TJob : IJob
        {
            _jobs.Add(typeof(TJob));
        }

        public async Task RunAsync(Type job)
        {
            if (!_jobs.Contains(job))
            {
                throw new ArgumentException($"Job {job.Name} not enqueued.");
            }

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
                    logger.LogError(error, $"Job '{job.Name}' failed during running.");
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

        public void Start()
        {
            if (!_options.Running) return;

            var timeZone = new TimeZoneOptions(_options.TimeZone).ToTimeZoneInfo();

            foreach (var job in Jobs)
            {
                var config = _options.Jobs.SingleOrDefault(entry => entry.Name == job.Name);

                if (config == null)
                {
                    var logger = _loggerFactory.CreateLogger(job.FullName);
                    logger.LogWarning($"No job configuration matches '{job.Name}'.");

                    continue;
                }

                if (!config.Running)
                {
                    continue;
                }

                var cron = new CronExpression(config.Cron);

                if (!cron.IsValid)
                {
                    var logger = _loggerFactory.CreateLogger(job.FullName);
                    logger.LogWarning($"Invalid cron expression for '{job.Name}'.");

                    continue;
                }

                var timer = new JobInterval(cron, timeZone, async () => await RunAsync(job));

                _timers.Add(timer);

                timer.Run();
            }
        }

        public void Stop()
        {
            foreach (var timer in _timers)
            {
                timer.Dispose();
            }

            _timers.Clear();
        }

        private void Restart(JobRunnerOptions options)
        {
            _options = options;

            Stop();
            Start();
        }
    }
}
