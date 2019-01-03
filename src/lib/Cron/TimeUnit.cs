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

namespace CronQuery.Cron
{
    internal class TimeUnit
    {
        private TimeUnit(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
        public bool IsSecond { get; private set; }
        public bool IsMinute { get; private set; }
        public bool IsHour { get; private set; }
        public bool IsDay { get; private set; }
        public bool IsMonth { get; private set; }
        public bool IsDayOfWeek { get; private set; }

        public static TimeUnit Second => new TimeUnit(0, 59) { IsSecond = true };
        public static TimeUnit Minute => new TimeUnit(0, 59) { IsMinute = true };
        public static TimeUnit Hour => new TimeUnit(0, 23) { IsHour = true };
        public static TimeUnit Day => new TimeUnit(1, 31) { IsDay = true };
        public static TimeUnit Month => new TimeUnit(1, 12) { IsMonth = true };
        public static TimeUnit DayOfWeek => new TimeUnit(0, 6) { IsDayOfWeek = true };
    }
}
