using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithDashTest
    {
        [Fact]
        public void ShouldGetNextSecond()
        {
            var expression = new CronExpression("10-30 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 15);
            var expected = new DateTime(2018, 12, 30, 08, 30, 16);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMinuteAfterLastSecond()
        {
            var expression = new CronExpression("10-30 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 30);
            var expected = new DateTime(2018, 12, 30, 08, 31, 10);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHourAfterLastMinute()
        {
            var expression = new CronExpression("* 10-30 * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 59);
            var expected = new DateTime(2018, 12, 30, 09, 10, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayAfterLastHour()
        {
            var expression = new CronExpression("* * 10-20 * * *");
            var current = new DateTime(2018, 12, 30, 20, 59, 59);
            var expected = new DateTime(2018, 12, 31, 10, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthAfterLastDay()
        {
            var expression = new CronExpression("* * * 10-20 * *");
            var current = new DateTime(2018, 12, 20, 23, 59, 59);
            var expected = new DateTime(2019, 01, 10, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextYearAfterLastMonth()
        {
            var expression = new CronExpression("* * * * 6-12 *");
            var current = new DateTime(2018, 12, 31, 23, 59, 59);
            var expected = new DateTime(2019, 06, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayOfWeek()
        {
            var expression = new CronExpression("* * * * * 1-5");
            var current = new DateTime(2018, 12, 28, 23, 59, 59);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextSecondCombinedWithComma()
        {
            var expression = new CronExpression("5,10-30 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 15);
            var expected = new DateTime(2018, 12, 30, 08, 30, 16);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMinuteAfterLastSecondCombinedWithComma()
        {
            var expression = new CronExpression("5,10-30 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 30);
            var expected = new DateTime(2018, 12, 30, 08, 31, 05);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHourAfterLastMinuteCombinedWithComma()
        {
            var expression = new CronExpression("* 5,10-30 * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 59);
            var expected = new DateTime(2018, 12, 30, 09, 05, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayAfterLastHourCombinedWithComma()
        {
            var expression = new CronExpression("* * 5,10-20 * * *");
            var current = new DateTime(2018, 12, 30, 20, 59, 59);
            var expected = new DateTime(2018, 12, 31, 05, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthAfterLastDayCombinedWithComma()
        {
            var expression = new CronExpression("* * * 5,10-20 * *");
            var current = new DateTime(2018, 12, 20, 23, 59, 59);
            var expected = new DateTime(2019, 01, 05, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextYearAfterLastMonthCombinedWithComma()
        {
            var expression = new CronExpression("* * * * 3,6-12 *");
            var current = new DateTime(2018, 12, 31, 23, 59, 59);
            var expected = new DateTime(2019, 03, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayOfWeekCombinedWithComma()
        {
            var expression = new CronExpression("* * * * * 0,1-5");
            var current = new DateTime(2018, 12, 28, 23, 59, 59);
            var expected = new DateTime(2018, 12, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
