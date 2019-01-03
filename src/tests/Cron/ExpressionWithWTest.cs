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
    public class ExpressionWithWTest
    {
        [Fact]
        public void ShouldGetNearestWeekdayFromSaturday()
        {
            var expression = new CronExpression("* * * 15W * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 14, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSaturday()
        {
            var expression = new CronExpression("* * * 15W * *");
            var current = new DateTime(2018, 12, 17, 23, 59, 59);
            var expected = new DateTime(2019, 01, 15, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSaturdayFirstDayOfMonth()
        {
            var expression = new CronExpression("* * * 1W * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 03, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSaturdayFirstDayOfMonth()
        {
            var expression = new CronExpression("* * * 1W * *");
            var current = new DateTime(2019, 03, 30, 23, 59, 59);
            var expected = new DateTime(2019, 04, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSunday()
        {
            var expression = new CronExpression("* * * 9W * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 10, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSunday()
        {
            var expression = new CronExpression("* * * 9W * *");
            var current = new DateTime(2018, 12, 10, 23, 59, 59);
            var expected = new DateTime(2019, 01, 09, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSundayLastDayOfMonth()
        {
            var expression = new CronExpression("* * * 31W * *");
            var current = new DateTime(2019, 03, 01, 23, 59, 59);
            var expected = new DateTime(2019, 03, 29, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSundayLastDayOfMonth()
        {
            var expression = new CronExpression("* * * 31W * *");
            var current = new DateTime(2019, 03, 31, 23, 59, 59);
            var expected = new DateTime(2019, 05, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
