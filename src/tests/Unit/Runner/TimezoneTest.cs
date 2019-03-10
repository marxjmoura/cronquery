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
using Xunit;
using Xunit.Abstractions;

namespace tests.Unit.Runner
{
	public class TimezoneTest
	{
		private readonly DateTime mockUtcTime = new DateTime(2000, 1, 1, 00, 00, 00);
		private readonly ITestOutputHelper output;

		public TimezoneTest(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Theory]
		[InlineData("00:00:00", "00:00:00")]
		[InlineData("01:00:00", "01:00:00")]
		[InlineData("-03:00:00", "-03:00:00")]
		[InlineData("01:30:00", "01:30:00")]
		[InlineData("01:54:00", "01:54:00")]
		public void CustomTimezoneShouldBeEqualToReal(string value, string expected)
		{
			var customTimezone = TimeZoneInfo.CreateCustomTimeZone("CronTime", TimeSpan.Parse(value), "Cron Timezone", "My Custom Cron Timezone");

			var resultCustom = TimeZoneInfo.ConvertTime(mockUtcTime, TimeZoneInfo.Utc, customTimezone);

			var timeSpan = resultCustom - mockUtcTime;
			var result = string.Format("{0:c}", timeSpan);

			output.WriteLine($"I expected {expected} and got {result}");
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("00:00:00", "00:00:00")]
		[InlineData("01:00:00", "01:00:00")]
		[InlineData("-03:00:00", "-03:00:00")]
		[InlineData("01:30:00", "01:30:00")]
		[InlineData("01:54:00", "01:54:00")]
		public void CalculateTimeSpansCorrectly(string value, string expected)
		{
			var timeSpan = TimeSpan.Parse(value);

			var result = string.Format("{0:c}", timeSpan);

			output.WriteLine($"I expected {expected} and got {result}");
			Assert.Equal(expected, result);
		}
	}
}
