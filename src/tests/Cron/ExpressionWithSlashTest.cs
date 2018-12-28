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
