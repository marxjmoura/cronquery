using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithLAndWTest
    {
        [Fact]
        public void ShouldGetNearestWeekdayFromWeekday()
        {
            var expression = new CronExpression("* * * LW * *");
            var current = new DateTime(2018, 12, 01, 08, 00, 00);
            var expected = new DateTime(2018, 12, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromWeekday()
        {
            var expression = new CronExpression("* * * LW * *");
            var current = new DateTime(2019, 01, 31, 23, 59, 59);
            var expected = new DateTime(2019, 02, 28, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSaturday()
        {
            var expression = new CronExpression("* * * LW * *");
            var current = new DateTime(2019, 08, 01, 08, 00, 00);
            var expected = new DateTime(2019, 08, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSaturday()
        {
            var expression = new CronExpression("* * * LW * *");
            var current = new DateTime(2019, 08, 30, 23, 59, 59);
            var expected = new DateTime(2019, 09, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSunday()
        {
            var expression = new CronExpression("* * * LW * *");
            var current = new DateTime(2019, 03, 01, 08, 00, 00);
            var expected = new DateTime(2019, 03, 29, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSunday()
        {
            var expression = new CronExpression("* * * LW * *");
            var current = new DateTime(2019, 03, 29, 23, 59, 59);
            var expected = new DateTime(2019, 04, 30, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
