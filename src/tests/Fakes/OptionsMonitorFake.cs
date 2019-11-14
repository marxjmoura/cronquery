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
using CronQuery.Mvc.Options;
using Microsoft.Extensions.Options;

namespace tests.Fakes
{
    public class OptionsMonitorFake : IOptionsMonitor<JobRunnerOptions>, IDisposable
    {
        private JobRunnerOptions _options;
        private Action<JobRunnerOptions, string> _listener;

        public OptionsMonitorFake(JobRunnerOptions options)
        {
            _options = options;
        }

        public JobRunnerOptions CurrentValue => _options;

        public void Change(Action<JobRunnerOptions> change)
        {
            change(_options);

            if (_listener != null)
            {
                _listener(_options, string.Empty);
            }
        }

        public void Dispose()
        {
            _listener = null;
        }

        public JobRunnerOptions Get(string name)
        {
            return _options;
        }

        public IDisposable OnChange(Action<JobRunnerOptions, string> listener)
        {
            _listener = listener;

            return this;
        }
    }
}
