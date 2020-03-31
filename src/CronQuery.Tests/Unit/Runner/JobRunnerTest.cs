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
using System.Threading.Tasks;
using CronQuery.API.Mvc.Jobs;
using CronQuery.Mvc.Jobs;
using Microsoft.Extensions.DependencyInjection;
using tests.Fakes;
using tests.Fakes.Jobs;
using Xunit;

namespace tests.Unit.Runner
{
    public class JobRunnerTest
    {
        private readonly LoggerFactoryFake _loggerFactory;
        private readonly ServiceCollection _serviceCollection;
        private readonly JobCollection _jobCollection;

        public JobRunnerTest()
        {
            _loggerFactory = new LoggerFactoryFake();
            _serviceCollection = new ServiceCollection();
            _jobCollection = new JobCollection(_serviceCollection);
        }

        [Fact]
        public void ShouldRunJobSuccessfully()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = _serviceCollection.AddSingleton<JobSuccessful>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            jobRunner.Start();

            Task.Delay(1500).GetAwaiter().GetResult(); // Waiting for the job

            Assert.True(serviceProvider.GetService<JobSuccessful>().Executed);
        }

        [Fact]
        public void ShouldNotRunStoppedJob()
        {
            var options = new OptionsMonitorFake(JobStopped.Options);
            var serviceProvider = _serviceCollection.AddSingleton<JobStopped>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            jobRunner.Start();

            Task.Delay(1500).GetAwaiter().GetResult(); // Waiting for the job

            Assert.False(serviceProvider.GetService<JobStopped>().Executed);
        }

        [Fact]
        public void ShouldLogErrorForJobNotEnqueued()
        {
            var options = new OptionsMonitorFake(JobNotEnqueued.Options);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            jobRunner.Start();

            Task.Delay(1500).GetAwaiter().GetResult(); // Waiting for the job

            Assert.Contains(_loggerFactory.Logger.Messages, message =>
                message == $"Job {nameof(JobNotEnqueued)} not enqueued.");
        }

        [Fact]
        public void ShouldLogJobFail()
        {
            var options = new OptionsMonitorFake(JobWithError.Options);
            var serviceProvider = _serviceCollection.AddSingleton<JobWithError>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            jobRunner.Start();

            Task.Delay(1500).GetAwaiter().GetResult(); // Waiting for the job

            Assert.Contains(_loggerFactory.Logger.Messages, message =>
                message == $"Job '{nameof(JobWithError)}' failed during running.");
        }

        [Fact]
        public void ShouldLogWhenCronIsInvalid()
        {
            var options = new OptionsMonitorFake(JobBadlyConfigured.Options);
            var serviceProvider = _serviceCollection.AddSingleton<JobBadlyConfigured>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            jobRunner.Start();

            Task.Delay(1500).GetAwaiter().GetResult(); // Waiting for the job

            Assert.Contains(_loggerFactory.Logger.Messages, message =>
                message == $"Invalid cron expression for '{nameof(JobBadlyConfigured)}'.");
        }

        [Fact]
        public void ShouldNotRunJobsWhenRunnerIsOff()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);
            options.Change(options => options.Running = false);

            var serviceProvider = _serviceCollection.AddSingleton<JobSuccessful>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            jobRunner.Start();

            Task.Delay(1500).GetAwaiter().GetResult(); // Waiting for the job

            Assert.False(serviceProvider.GetService<JobSuccessful>().Executed);
        }

        [Fact]
        public void ShouldAssumeNewConfigurationImmediately()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = _serviceCollection.AddSingleton<JobSuccessful>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            jobRunner.Start();

            options.Change(options => options.Running = false);

            Task.Delay(1500).GetAwaiter().GetResult(); // Waiting for the job

            Assert.False(serviceProvider.GetService<JobSuccessful>().Executed);
        }

        [Fact]
        public void ShouldNotInstanceWithNullOptions()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new JobRunner(null, _jobCollection, serviceProvider, _loggerFactory);
            });
        }

        [Fact]
        public void ShouldNotInstanceWithNullJobCollection()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new JobRunner(options, null, serviceProvider, _loggerFactory);
            });
        }

        [Fact]
        public void ShouldNotInstanceWithNullServiceProvider()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);

            Assert.Throws<ArgumentNullException>(() =>
            {
                new JobRunner(options, _jobCollection, null, _loggerFactory);
            });
        }

        [Fact]
        public void ShouldNotInstanceWithNullLoggerFactory()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = new ServiceProviderFake();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new JobRunner(options, _jobCollection, serviceProvider, null);
            });
        }

        [Fact]
        public async Task ShouldRunJobManually()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = _serviceCollection.AddSingleton<JobSuccessful>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            await jobRunner.RunAsync(nameof(JobSuccessful));

            Assert.True(serviceProvider.GetService<JobSuccessful>().Executed);
        }

        [Fact]
        public async Task ShouldLogErrorWhenRunManuallyJobNotEnqueued()
        {
            var options = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            await jobRunner.RunAsync(nameof(JobNotEnqueued));

            Assert.Contains(_loggerFactory.Logger.Messages, message =>
                message == $"Job {nameof(JobNotEnqueued)} not enqueued.");
        }

        [Fact]
        public async Task ShouldLogJobFailAfterRunManually()
        {
            var options = new OptionsMonitorFake(JobWithError.Options);
            var serviceProvider = _serviceCollection.AddSingleton<JobWithError>().BuildServiceProvider();
            var jobRunner = new JobRunner(options, _jobCollection, serviceProvider, _loggerFactory);

            await jobRunner.RunAsync(nameof(JobWithError));

            Assert.Contains(_loggerFactory.Logger.Messages, message =>
                message == $"Job '{nameof(JobWithError)}' failed during running.");
        }
    }
}
