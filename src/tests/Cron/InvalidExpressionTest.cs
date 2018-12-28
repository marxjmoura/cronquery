using System;
using CronQuery.Cron;
using Xunit;

namespace tests.Cron
{
    public class InvalidExpressionTest
    {
        [Fact]
        public void ShouldNotEvaluateLowerThan6Fields()
        {
            var expression = new CronExpression("* * * * *");
            var current = DateTime.UtcNow;
            var expected = DateTime.MinValue;

            Assert.False(expression.IsValid);
            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldNotEvaluateMoreThan6Fields()
        {
            var expression = new CronExpression("* * * * * * *");
            var current = DateTime.UtcNow;
            var expected = DateTime.MinValue;

            Assert.False(expression.IsValid);
            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldNotEvaluateInvalidExpression()
        {
            var expression = new CronExpression("IN V A L I D");
            var current = DateTime.UtcNow;
            var expected = DateTime.MinValue;

            Assert.False(expression.IsValid);
            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldIgnoreSecondOutOfRange()
        {
            var expression = new CronExpression("99 * * * * *");
            var current = new DateTime(2019, 01, 01, 00, 00, 00);
            var expected = new DateTime(2019, 01, 01, 00, 00, 59);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldIgnoreIncrementByZero()
        {
            var expression = new CronExpression("*/0 * * * * *");
            var current = new DateTime(2019, 01, 01, 00, 00, 00);
            var expected = new DateTime(2019, 01, 01, 00, 00, 01);

            Assert.Equal(expected, expression.Next(current));
        }

        [Fact]
        public void ShouldIgnoreCharacterNotAllowed()
        {
            var expression = new CronExpression("* * 10#5 * * *");
            var current = new DateTime(2019, 01, 01, 00, 00, 00);
            var expected = DateTime.MinValue;

            Assert.Equal(expected, expression.Next(current));
        }
    }
}
