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
using CronQuery.Cron;
using CronQuery.Mvc.Jobs;
using Xunit;

namespace tests.Unit.Runner
{
    public class JobIntervalTest
    {
        [Fact]
        public async Task ShouldRunWork()
        {
            var fires = 0;
            var cron = new CronExpression("* * * * * *");
            var interval = new JobInterval(cron, TimeZoneInfo.Utc, () => Task.FromResult(++fires));

            interval.Run();

            await Task.Delay(2500);

            Assert.Equal(2, fires);
        }

        [Fact]
        public async Task ShouldNotRunWorkAfterDispose()
        {
            var fires = 0;
            var cron = new CronExpression("* * * * * *");
            var interval = new JobInterval(cron, TimeZoneInfo.Utc, () => Task.FromResult(++fires));

            interval.Run();

            await Task.Delay(1500);

            interval.Dispose();

            await Task.Delay(1500);

            Assert.Equal(1, fires);
        }

        [Fact]
        public void ShouldNotRunWorkForUnreachableCondition()
        {
            var fires = 0;
            var cron = new CronExpression("* * * 30 2 *");
            var interval = new JobInterval(cron, TimeZoneInfo.Utc, () => Task.FromResult(++fires));

            interval.Run();

            Assert.Equal(0, fires);
        }

        [Fact]
        public void ShouldNotInstanceWithNullCron()
        {
            Assert.Throws<ArgumentNullException>(() => new JobInterval(null, TimeZoneInfo.Utc, () => Task.CompletedTask));
        }

        [Fact]
        public void ShouldNotInstanceWithNullNullTimezone()
        {
            var cron = new CronExpression("* * * 30 2 *");

            Assert.Throws<ArgumentNullException>(() => new JobInterval(cron, null, () => Task.CompletedTask));
        }

        [Fact]
        public void ShouldNotInstanceWithNullWork()
        {
            var cron = new CronExpression("* * * 30 2 *");

            Assert.Throws<ArgumentNullException>(() => new JobInterval(cron, TimeZoneInfo.Utc, null));
        }
    }
}
