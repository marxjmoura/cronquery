using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithDashAndSlashTest
    {
        [Fact]
        public void ShouldGetNextSecond()
        {
            var expression = new CronExpression("20-50/10 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 15, 30);
            var expected = new DateTime(2018, 12, 30, 08, 15, 40);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMinuteAfterLastSecond()
        {
            var expression = new CronExpression("20-50/10 * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 15, 50);
            var expected = new DateTime(2018, 12, 30, 08, 16, 20);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMinute()
        {
            var expression = new CronExpression("* 20-50/10 * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 59);
            var expected = new DateTime(2018, 12, 30, 08, 40, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHourAfterLastMinute()
        {
            var expression = new CronExpression("* 20-50/10 * * * *");
            var current = new DateTime(2018, 12, 30, 08, 50, 59);
            var expected = new DateTime(2018, 12, 30, 09, 20, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHour()
        {
            var expression = new CronExpression("* * 8-18/2 * * *");
            var current = new DateTime(2018, 12, 30, 08, 59, 59);
            var expected = new DateTime(2018, 12, 30, 10, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayAfterLastHour()
        {
            var expression = new CronExpression("* * 8-18/2 * * *");
            var current = new DateTime(2018, 12, 30, 18, 59, 59);
            var expected = new DateTime(2018, 12, 31, 08, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDay()
        {
            var expression = new CronExpression("* * * 5-25/5 * *");
            var current = new DateTime(2018, 12, 10, 23, 59, 59);
            var expected = new DateTime(2018, 12, 15, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthAfterLastDay()
        {
            var expression = new CronExpression("* * * 5-25/5 * *");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2019, 01, 05, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonth()
        {
            var expression = new CronExpression("* * * * 2-12/2 *");
            var current = new DateTime(2018, 12, 31, 23, 59, 59);
            var expected = new DateTime(2019, 02, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayOfWeek()
        {
            var expression = new CronExpression("* * * * * 1-5/2");
            var current = new DateTime(2018, 12, 28, 23, 59, 59);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
