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
using CronQuery.Cron;
using Xunit;

namespace CronQuery.Tests.Unit.Cron
{
    public class ExpressionWithLTest
    {
        [Fact]
        public void ShouldGetNextLastDayInJanuary()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 01, 01, 23, 59, 59);
            var expected = new DateTime(2018, 01, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromJanuary()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 01, 31, 23, 59, 59);
            var expected = new DateTime(2018, 02, 28, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromJanuaryInLeapYear()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2020, 01, 31, 23, 59, 59);
            var expected = new DateTime(2020, 02, 29, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInFebruary()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 02, 01, 23, 59, 59);
            var expected = new DateTime(2018, 02, 28, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInFebruaryInLeapYear()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2020, 02, 01, 23, 59, 59);
            var expected = new DateTime(2020, 02, 29, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromFebruary()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 02, 28, 23, 59, 59);
            var expected = new DateTime(2018, 03, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromFebruaryInLeapYear()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2020, 02, 29, 23, 59, 59);
            var expected = new DateTime(2020, 03, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInMarch()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 03, 01, 23, 59, 59);
            var expected = new DateTime(2018, 03, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromMarch()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 03, 31, 23, 59, 59);
            var expected = new DateTime(2018, 04, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInApril()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 04, 01, 23, 59, 59);
            var expected = new DateTime(2018, 04, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromApril()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 04, 30, 23, 59, 59);
            var expected = new DateTime(2018, 05, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInMay()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 05, 01, 23, 59, 59);
            var expected = new DateTime(2018, 05, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromMay()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 05, 31, 23, 59, 59);
            var expected = new DateTime(2018, 06, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInJune()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 06, 01, 23, 59, 59);
            var expected = new DateTime(2018, 06, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromJune()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 06, 30, 23, 59, 59);
            var expected = new DateTime(2018, 07, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInJuly()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 07, 01, 23, 59, 59);
            var expected = new DateTime(2018, 07, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromJuly()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 07, 31, 23, 59, 59);
            var expected = new DateTime(2018, 08, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInAugust()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 08, 01, 23, 59, 59);
            var expected = new DateTime(2018, 08, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromAugust()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 08, 31, 23, 59, 59);
            var expected = new DateTime(2018, 09, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInSeptember()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 09, 01, 23, 59, 59);
            var expected = new DateTime(2018, 09, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromSeptember()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 09, 30, 23, 59, 59);
            var expected = new DateTime(2018, 10, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInOctober()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 10, 01, 23, 59, 59);
            var expected = new DateTime(2018, 10, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromOctober()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 10, 31, 23, 59, 59);
            var expected = new DateTime(2018, 11, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInNovember()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 11, 01, 23, 59, 59);
            var expected = new DateTime(2018, 11, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromNovember()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 11, 30, 23, 59, 59);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayInDecember()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastDayFromDecember()
        {
            var expression = new CronExpression("* * * L * *");
            var current = new DateTime(2018, 12, 31, 23, 59, 59);
            var expected = new DateTime(2019, 01, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayBeforeLastDayOfMonth()
        {
            var expression = new CronExpression("* * * L-3 * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 28, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastSunday()
        {
            var expression = new CronExpression("* * * * * 0L");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2018, 12, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastMonday()
        {
            var expression = new CronExpression("* * * * * 1L");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastTuesday()
        {
            var expression = new CronExpression("* * * * * 2L");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2019, 01, 29, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastWednesday()
        {
            var expression = new CronExpression("* * * * * 3L");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2018, 12, 26, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastThursday()
        {
            var expression = new CronExpression("* * * * * 4L");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2018, 12, 27, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastFriday()
        {
            var expression = new CronExpression("* * * * * 5L");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2018, 12, 28, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextLastSaturday()
        {
            var expression = new CronExpression("* * * * * 6L");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2018, 12, 29, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
