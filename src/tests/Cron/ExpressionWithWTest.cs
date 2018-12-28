using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class ExpressionWithWTest
    {
        [Fact]
        public void ShouldGetNearestWeekdayFromSaturday()
        {
            var expression = new CronExpression("* * * 15W * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 14, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSaturday()
        {
            var expression = new CronExpression("* * * 15W * *");
            var current = new DateTime(2018, 12, 17, 23, 59, 59);
            var expected = new DateTime(2019, 01, 15, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSaturdayFirstDayOfMonth()
        {
            var expression = new CronExpression("* * * 1W * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 03, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSaturdayFirstDayOfMonth()
        {
            var expression = new CronExpression("* * * 1W * *");
            var current = new DateTime(2019, 03, 30, 23, 59, 59);
            var expected = new DateTime(2019, 04, 01, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSunday()
        {
            var expression = new CronExpression("* * * 9W * *");
            var current = new DateTime(2018, 12, 01, 23, 59, 59);
            var expected = new DateTime(2018, 12, 10, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSunday()
        {
            var expression = new CronExpression("* * * 9W * *");
            var current = new DateTime(2018, 12, 10, 23, 59, 59);
            var expected = new DateTime(2019, 01, 09, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNearestWeekdayFromSundayLastDayOfMonth()
        {
            var expression = new CronExpression("* * * 31W * *");
            var current = new DateTime(2019, 03, 01, 23, 59, 59);
            var expected = new DateTime(2019, 03, 29, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldGetNextMonthNearestWeekdayFromSundayLastDayOfMonth()
        {
            var expression = new CronExpression("* * * 31W * *");
            var current = new DateTime(2019, 03, 31, 23, 59, 59);
            var expected = new DateTime(2019, 05, 31, 00, 00, 00);

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
