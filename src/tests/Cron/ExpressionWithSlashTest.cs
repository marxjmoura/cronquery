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
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithSlashTest
    {
        [Fact]
        public void ShouldGetNextSecond()
        {
            var expression = new CronExpression("*/10 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 15, 20);
            var expected = new DateTime(2018, 12, 30, 08, 15, 30);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMinuteAfterLastSecond()
        {
            var expression = new CronExpression("*/10 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 15, 50);
            var expected = new DateTime(2018, 12, 30, 08, 16, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHourAfterLastMinute()
        {
            var expression = new CronExpression("* */10 * * * *");
            var current = new DateTime(2018, 12, 30, 08, 50, 59);
            var expected = new DateTime(2018, 12, 30, 09, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayAfterLastHour()
        {
            var expression = new CronExpression("* * */10 * * *");
            var current = new DateTime(2018, 12, 30, 20, 59, 59);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthAfterLastDay()
        {
            var expression = new CronExpression("* * * */7 * *");
            var current = new DateTime(2018, 12, 29, 23, 59, 59);
            var expected = new DateTime(2019, 01, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextYearAfterLastMonth()
        {
            var expression = new CronExpression("* * * * */6 *");
            var current = new DateTime(2018, 07, 31, 23, 59, 59);
            var expected = new DateTime(2019, 01, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayOfWeek()
        {
            var expression = new CronExpression("* * * * * */2");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2018, 12, 27, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
