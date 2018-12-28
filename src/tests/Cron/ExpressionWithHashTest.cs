using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithHashTest
    {
        [Fact]
        public void ShouldGetNextDayOfWeekInTheSameMonth()
        {
            var expression = new CronExpression("* * * * * 1#3");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 17, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextDayOfWeekInTheNextMonth()
        {
            var expression = new CronExpression("* * * * * 1#3");
            var current = new DateTime(2018, 12, 25, 23, 59, 59);
            var expected = new DateTime(2019, 01, 21, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
