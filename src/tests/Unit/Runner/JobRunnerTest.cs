/* MIT License
 *
 * Copyright (c) 2018 LogiQ System
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
using CronQuery.Mvc.Jobs;
using Microsoft.Extensions.DependencyInjection;
using tests.Fakes;
using tests.Fakes.Jobs;
using Xunit;

namespace tests.Unit.Runner
{
    public class JobRunnerTest
    {
        [Fact]
        public async Task ShouldRunJobSuccessfully()
        {
            var optionsMonitor = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();
            var jobRunner = new JobRunner(optionsMonitor, serviceProvider, loggerFactory);

            jobRunner.Enqueue<JobSuccessful>();
            jobRunner.Start();

            await Task.Delay(1500); // Waiting for the job

            Assert.True(serviceProvider.GetService<JobSuccessful>().Executed);
        }

        [Fact]
        public async Task ShouldNotRunStoppedJob()
        {
            var optionsMonitor = new OptionsMonitorFake(JobStopped.Options);
            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();
            var jobRunner = new JobRunner(optionsMonitor, serviceProvider, loggerFactory);

            jobRunner.Enqueue<JobStopped>();
            jobRunner.Start();

            await Task.Delay(1500); // Waiting for the job

            Assert.False(serviceProvider.GetService<JobStopped>().Executed);
        }

        [Fact]
        public void ShouldLogJobNotConfigured()
        {
            var optionsMonitor = new OptionsMonitorFake(JobNotConfigured.Options);
            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();
            var jobRunner = new JobRunner(optionsMonitor, serviceProvider, loggerFactory);

            jobRunner.Enqueue<JobNotConfigured>();
            jobRunner.Start();

            Assert.Contains(loggerFactory.Logger.Messages, message =>
                message == $"No job configuration matches '{nameof(JobNotConfigured)}'.");
        }

        [Fact]
        public async Task ShouldLogJobFail()
        {
            var optionsMonitor = new OptionsMonitorFake(JobWithError.Options);
            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();
            var jobRunner = new JobRunner(optionsMonitor, serviceProvider, loggerFactory);

            jobRunner.Enqueue<JobWithError>();
            jobRunner.Start();

            await Task.Delay(1500); // Waiting for the job

            Assert.Contains(loggerFactory.Logger.Messages, message =>
                message == $"Job '{nameof(JobWithError)}' failed during running.");
        }

        [Fact]
        public async Task ShouldLogWhenCronIsInvalid()
        {
            var optionsMonitor = new OptionsMonitorFake(JobBadlyConfigured.Options);
            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();
            var jobRunner = new JobRunner(optionsMonitor, serviceProvider, loggerFactory);

            jobRunner.Enqueue<JobBadlyConfigured>();
            jobRunner.Start();

            await Task.Delay(1500); // Waiting for the job

            Assert.Contains(loggerFactory.Logger.Messages, message =>
                message == $"Invalid cron expression for '{nameof(JobBadlyConfigured)}'.");
        }

        [Fact]
        public async Task ShouldNotRunJobsWhenRunnerIsOff()
        {
            var optionsMonitor = new OptionsMonitorFake(JobSuccessful.Options);
            optionsMonitor.Change(options => options.Running = false);

            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();
            var jobRunner = new JobRunner(optionsMonitor, serviceProvider, loggerFactory);

            jobRunner.Enqueue<JobSuccessful>();
            jobRunner.Start();

            await Task.Delay(1500); // Waiting for the job

            Assert.False(serviceProvider.GetService<JobSuccessful>().Executed);
        }

        [Fact]
        public async Task ShouldAssumeNewConfigurationImmediately()
        {
            var optionsMonitor = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();
            var jobRunner = new JobRunner(optionsMonitor, serviceProvider, loggerFactory);

            jobRunner.Enqueue<JobSuccessful>();
            jobRunner.Start();

            optionsMonitor.Change(options => options.Running = false);

            await Task.Delay(1500); // Waiting for the job

            Assert.False(serviceProvider.GetService<JobSuccessful>().Executed);
        }

        [Fact]
        public void ShouldNotInstanceWithNullOptions()
        {
            var serviceProvider = new ServiceProviderFake();
            var loggerFactory = new LoggerFactoryFake();

            Assert.Throws<ArgumentNullException>(() => new JobRunner(null, serviceProvider, loggerFactory));
        }

        [Fact]
        public void ShouldNotInstanceWithNullServiceProvider()
        {
            var optionsMonitor = new OptionsMonitorFake(JobSuccessful.Options);
            var loggerFactory = new LoggerFactoryFake();

            Assert.Throws<ArgumentNullException>(() => new JobRunner(optionsMonitor, null, loggerFactory));
        }

        [Fact]
        public void ShouldNotInstanceWithNullLoggerFactory()
        {
            var optionsMonitor = new OptionsMonitorFake(JobSuccessful.Options);
            var serviceProvider = new ServiceProviderFake();

            Assert.Throws<ArgumentNullException>(() => new JobRunner(optionsMonitor, serviceProvider, null));
        }
    }
}
