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
using System.Threading;
using System.Threading.Tasks;
using CronQuery.API.Mvc.Jobs;
using CronQuery.Cron;
using CronQuery.Mvc.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CronQuery.Mvc.Jobs
{
    public sealed class JobRunner : IHostedService
    {
        private readonly ICollection<IDisposable> _timers;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly JobCollection _jobs;

        private JobRunnerOptions _options;

        public JobRunner(IOptionsMonitor<JobRunnerOptions> options, JobCollection jobs,
            IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _timers = new List<IDisposable>();
            _options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _jobs = jobs ?? throw new ArgumentNullException(nameof(jobs));

            options.OnChange(Restart);
        }

        public async Task RunAsync(string jobName)
        {
            var service = _jobs.SingleOrDefault(service => service.ServiceType.Name == jobName);

            if (service == null)
            {
                _loggerFactory.CreateLogger(GetType().FullName)
                    .LogWarning($"Job {jobName} is not in the queue.");

                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var jobInstance = ((IJob)scope.ServiceProvider.GetRequiredService(service.ServiceType));

                try
                {
                    await jobInstance.RunAsync();
                }
                catch (Exception error)
                {
                    _loggerFactory.CreateLogger(GetType().FullName)
                        .LogError(error, $"Job '{service.ServiceType.Name}' failed during running.");
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

            foreach (var job in _options.Jobs)
            {
                if (!job.Running)
                {
                    continue;
                }

                var cron = new CronExpression(job.Cron);

                if (!cron.IsValid)
                {
                    _loggerFactory.CreateLogger(GetType().FullName)
                        .LogWarning($"Invalid cron expression for '{job.Name}'.");

                    continue;
                }

                var timer = new JobInterval(cron, timeZone, async () => await RunAsync(job.Name));

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

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            Start();

            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            Stop();

            return Task.CompletedTask;
        }
    }
}
