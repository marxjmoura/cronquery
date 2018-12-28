using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithCommaTest
    {
        [Fact]
        public void ShouldGetNextMinute()
        {
            var expression = new CronExpression("10,20,30 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 10);
            var expected = new DateTime(2018, 12, 30, 08, 30, 20);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMinuteAfterLastSecond()
        {
            var expression = new CronExpression("10,20,30 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 30);
            var expected = new DateTime(2018, 12, 30, 08, 31, 10);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHourAfterLastMinute()
        {
            var expression = new CronExpression("* 10,20,30 * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 59);
            var expected = new DateTime(2018, 12, 30, 09, 10, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayAfterLastHour()
        {
            var expression = new CronExpression("* * 6,18 * * *");
            var current = new DateTime(2018, 12, 30, 18, 59, 59);
            var expected = new DateTime(2018, 12, 31, 06, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthAfterLastDay()
        {
            var expression = new CronExpression("* * * 15,20,25 * *");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2019, 01, 15, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
