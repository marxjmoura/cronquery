using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class UnreachableConditionTest
    {
        [Fact]
        public void ShouldNotEvaluateNearestWeekdayOnlyOnSunday()
        {
            var expression = new CronExpression("0 0 8 15W * 0");
            var current = new DateTime(2019, 01, 01, 00, 00, 00);
            var expected = DateTime.MinValue;

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldNotEvaluateOnlyMonthsThatNotReachTheGivenDay()
        {
            var expression = new CronExpression("0 0 8 31 2,4,6 *");
            var current = new DateTime(2019, 01, 01, 00, 00, 00);
            var expected = DateTime.MinValue;

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
