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

using CronQuery.Mvc.DependencyInjection;
using CronQuery.Tests.Fakes;
using CronQuery.Tests.Fakes.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public static class TestProgram
{
    public static TestServer CreateServer()
    {
        var host = new WebHostBuilder()
            .UseEnvironment("Testing")
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                var targetFramework = "net6.0";
                var mode = Debugger.IsAttached ? "Debug" : "Release";
                var buildPath = Path.Combine("bin", mode, targetFramework);
                var projectPath = AppContext.BaseDirectory.Replace($"{buildPath}/", string.Empty);
                var appsettings = Path.Combine(projectPath, "Functional", "appsettings.json");

                config.AddJsonFile(appsettings, optional: false, reloadOnChange: true);
            })
            .ConfigureServices((builder, services) =>
            {
                services.AddRouting();

                services.AddCronQuery(builder.Configuration.GetSection("CronQuery"));

                services.AddSingleton<ILoggerFactory, LoggerFactoryFake>();

                services.AddSingleton<JobSuccessful>();
                services.AddSingleton<JobBadlyConfigured>();
                services.AddSingleton<JobWithError>();
                services.AddSingleton<JobStopped>();
            })
            .Configure(builder =>
            {
                builder.UseRouting();
                builder.UseEndpoints(api => api.MapGet("/", () => "Testing jobs..."));
            });

        return new(host);
    }
}
