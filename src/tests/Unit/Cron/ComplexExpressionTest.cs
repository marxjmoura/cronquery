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

namespace tests.Unit.Cron
{
    public class ComplexExpressionTest
    {
        [Fact]
        public void ShouldGetNextExactTimeInList()
        {
            var expression = new CronExpression("0 0 8 10,20,30 2-12/2 *");
            var current = new DateTime(2019, 02, 20, 23, 59, 59);
            var expected = new DateTime(2019, 04, 10, 08, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextExactTimeOnTheLastDay()
        {
            var expression = new CronExpression("0 30 14 L * *");
            var current = new DateTime(2019, 01, 31, 15, 26, 34);
            var expected = new DateTime(2019, 02, 28, 14, 30, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextExactTimeOnTheLastDayAndWeekday()
        {
            var expression = new CronExpression("0 0 8 LW * *");
            var current = new DateTime(2019, 02, 20, 23, 59, 59);
            var expected = new DateTime(2019, 02, 28, 08, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At12PmEveryDay()
        {
            var expression = new CronExpression("0 0 12 * * *");
            var current = new DateTime(2018, 12, 31, 10, 36, 35);
            var expected = new DateTime(2018, 12, 31, 12, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void Every5MinutesFrom1PmAndFrom6Pm()
        {
            var expression = new CronExpression("0 0/5 13,18 * * *");
            var current = new DateTime(2018, 02, 08, 02, 46, 03);
            var expected = new DateTime(2018, 02, 08, 13, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At115PmAnd145PmEveryTuesdayInJune()
        {
            var expression = new CronExpression("0 15,45 13 * 6 2");
            var current = new DateTime(2018, 12, 24, 17, 03, 53);
            var expected = new DateTime(2019, 06, 04, 13, 15, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At930EveryDayExceptSunday()
        {
            var expression = new CronExpression("0 30 9 * * 1-5");
            var current = new DateTime(2018, 09, 21, 20, 26, 07);
            var expected = new DateTime(2018, 09, 24, 09, 30, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At930On15ThEveryMonth()
        {
            var expression = new CronExpression("0 30 9 15 * *");
            var current = new DateTime(2018, 10, 07, 01, 43, 36);
            var expected = new DateTime(2018, 10, 15, 09, 30, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At6PmOnTheLastDayEveryMonth()
        {
            var expression = new CronExpression("0 0 18 L * *");
            var current = new DateTime(2018, 11, 12, 11, 19, 15);
            var expected = new DateTime(2018, 11, 30, 18, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At6PmOnThe3ThLastDayEveryMonth()
        {
            var expression = new CronExpression("0 0 18 L-3 * *");
            var current = new DateTime(2018, 07, 25, 00, 09, 59);
            var expected = new DateTime(2018, 07, 28, 18, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At1030OnTheLastFridayEveryMonth()
        {
            var expression = new CronExpression("0 30 10 * * 5L");
            var current = new DateTime(2018, 12, 31, 02, 34, 05);
            var expected = new DateTime(2019, 01, 25, 10, 30, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At10AmOnTheThirdMondayEveryMonth()
        {
            var expression = new CronExpression("0 0 10 * * 1#3");
            var current = new DateTime(2018, 12, 04, 20, 56, 31);
            var expected = new DateTime(2018, 12, 17, 10, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void At12AmEvery5DaysStartingOn10Th()
        {
            var expression = new CronExpression("0 0 0 10/5 * *");
            var current = new DateTime(2019, 01, 13, 05, 17, 15);
            var expected = new DateTime(2019, 01, 15, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void Every5MinutesSpecificTimeAndDays()
        {
            var expression = new CronExpression("0 0/5 14,18,3-39,52 * 1,3,9 1-5");
            var current = new DateTime(2019, 05, 21, 08, 12, 39);
            var expected = new DateTime(2019, 09, 02, 03, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
