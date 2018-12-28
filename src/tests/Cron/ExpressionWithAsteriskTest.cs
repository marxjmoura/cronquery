using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithAsteriskTest
    {
        [Fact]
        public void ShouldGetNextSecond()
        {
            var expression = new CronExpression("* * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 20);
            var expected = new DateTime(2018, 12, 30, 08, 30, 21);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMinute()
        {
            var expression = new CronExpression("* * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 30, 59);
            var expected = new DateTime(2018, 12, 30, 08, 31, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextHour()
        {
            var expression = new CronExpression("* * * * * *");
            var current = new DateTime(2018, 12, 30, 08, 59, 59);
            var expected = new DateTime(2018, 12, 30, 09, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDay()
        {
            var expression = new CronExpression("* * * * * *");
            var current = new DateTime(2018, 12, 30, 23, 59, 59);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonth()
        {
            var expression = new CronExpression("* * * * * *");
            var current = new DateTime(2018, 12, 31, 23, 59, 59);
            var expected = new DateTime(2019, 01, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
