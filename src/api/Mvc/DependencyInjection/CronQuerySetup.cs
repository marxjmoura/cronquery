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

using CronQuery.Mvc.Jobs;
using Microsoft.Extensions.Hosting;

namespace CronQuery.Mvc.DependencyInjection
{
	public sealed class CronQuerySetup
    {
        private readonly JobRunner runner;

        internal CronQuerySetup(JobRunner runner)
        {
            this.runner = runner;
        }

        public CronQuerySetup Enqueue<TJob>() where TJob : IJob
        {
            runner.Enqueue<TJob>();

            return this;
        }

        public void StartWith(IHostApplicationLifetime appLifetime)
        {
            appLifetime.ApplicationStarted.Register(runner.Start);
            appLifetime.ApplicationStopping.Register(runner.Stop);
        }
    }
}
