using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithDigitTest
    {
        [Fact]
        public void ShouldGetNextMinuteAfterLastSecond()
        {
            var expression = new CronExpression("30 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 30);
            var expected = new DateTime(2018, 12, 30, 08, 31, 30);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHourAfterLastMinute()
        {
            var expression = new CronExpression("* 30 * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 59);
            var expected = new DateTime(2018, 12, 30, 09, 30, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayAfterLastHour()
        {
            var expression = new CronExpression("* * 8 * * *");
            var current = new DateTime(2018, 12, 30, 23, 59, 59);
            var expected = new DateTime(2018, 12, 31, 08, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthAfterLastDay()
        {
            var expression = new CronExpression("* * * 15 * *");
            var current = new DateTime(2018, 12, 31, 23, 59, 59);
            var expected = new DateTime(2019, 01, 15, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthAfterLastDayInMonth()
        {
            var expression = new CronExpression("* * * 31 * *");
            var current = new DateTime(2019, 01, 31, 23, 59, 59);
            var expected = new DateTime(2019, 03, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextYearAfterLastMonth()
        {
            var expression = new CronExpression("* * * * 6 *");
            var current = new DateTime(2018, 12, 31, 23, 59, 59);
            var expected = new DateTime(2019, 06, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayOfWeek()
        {
            var expression = new CronExpression("* * * * * 3");
            var current = new DateTime(2018, 12, 28, 23, 59, 59);
            var expected = new DateTime(2019, 01, 02, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
