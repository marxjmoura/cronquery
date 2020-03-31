/*
 * MIT License
 *
 * Copyright (c) 2018 Marx J. Moura
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using CronQuery.Cron;
using Xunit;

namespace CronQuery.Tests.Unit.Cron
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
