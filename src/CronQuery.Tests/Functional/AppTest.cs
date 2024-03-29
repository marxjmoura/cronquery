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

namespace CronQuery.Tests.Functional;

using CronQuery.Tests.Fakes;
using CronQuery.Tests.Fakes.Jobs;
using Xunit;

public sealed class AppTest
{
    [Fact]
    public async Task Run()
    {
        var server = TestProgram.CreateServer();

        await Task.Delay(1500); // Waiting for the jobs
        await server.Host.StopAsync();

        Assert.True(server.Job<JobSuccessful>().Executed);
        Assert.False(server.Job<JobStopped>().Executed);

        Assert.Contains(server.Logger().Messages, message =>
            message == $"Job '{nameof(JobWithError)}' failed during running.");

        Assert.Contains(server.Logger().Messages, message =>
            message == $"Job {nameof(JobNotEnqueued)} is not in the queue.");

        Assert.Contains(server.Logger().Messages, message =>
            message == $"Invalid cron expression for '{nameof(JobBadlyConfigured)}'.");
    }
}
