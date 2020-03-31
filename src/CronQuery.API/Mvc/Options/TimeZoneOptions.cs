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
using System.Text.RegularExpressions;

namespace CronQuery.Mvc.Options
{
    public class TimeZoneOptions
    {
        const string UtcOffsetRegex = @"^UTC[+-]\d{2}:\d{2}$";

        private readonly string _timeZone;

        public TimeZoneOptions(string timeZone)
        {
            _timeZone = timeZone;
        }

        public TimeZoneInfo ToTimeZoneInfo()
        {
            if (string.IsNullOrWhiteSpace(_timeZone))
            {
                return TimeZoneInfo.Utc;
            }
            else if (Regex.IsMatch(_timeZone, UtcOffsetRegex))
            {
                var timeSpanString = Regex.Replace(_timeZone, "UTC[+]?", string.Empty);

                return TimeZoneInfo.CreateCustomTimeZone(
                    id: "CronQuery",
                    baseUtcOffset: TimeSpan.Parse(timeSpanString),
                    displayName: $"({_timeZone}) CronQuery",
                    standardDisplayName: "CronQuery Custom Time"
                );
            }
            else
            {
                return TimeZoneInfo.FindSystemTimeZoneById(_timeZone);
            }
        }
    }
}
