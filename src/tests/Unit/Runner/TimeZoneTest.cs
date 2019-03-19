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
using CronQuery.Mvc.Options;
using Xunit;
using Xunit.Abstractions;

namespace tests.Unit.Runner
{
    public class TimeZoneTest
    {
        private readonly DateTime _mockUtcTime;

        public TimeZoneTest(ITestOutputHelper output)
        {
            _mockUtcTime = new DateTime(2000, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        }

        [Theory]
        [InlineData(null, "2000-01-01T00:00:00")]
        [InlineData("", "2000-01-01T00:00:00")]
        [InlineData(" ", "2000-01-01T00:00:00")]
        [InlineData("UTC+00:00", "2000-01-01T00:00:00")]
        [InlineData("UTC+01:00", "2000-01-01T01:00:00")]
        [InlineData("UTC-03:00", "1999-12-31T21:00:00")]
        [InlineData("UTC+01:30", "2000-01-01T01:30:00")]
        [InlineData("Europe/Budapest", "2000-01-01T01:00:00")]
        [InlineData("America/Sao_Paulo", "1999-12-31T22:00:00")]
        public void ShouldConvertToLocalTime(string timeZone, string expected)
        {
            var timeZoneOptions = new TimeZoneOptions(timeZone);
            var timeZoneInfo = timeZoneOptions.ToTimeZoneInfo();
            var localDateTime = TimeZoneInfo.ConvertTime(_mockUtcTime, timeZoneInfo);
            var iso8601Format = localDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            Assert.Equal(expected, iso8601Format);
        }
    }
}
