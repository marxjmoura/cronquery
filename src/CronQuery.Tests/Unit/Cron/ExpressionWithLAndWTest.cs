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

namespace CronQuery.Tests.Unit.Cron;

using CronQuery.Cron;
using Xunit;

public sealed class ExpressionWithLAndWTest
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
