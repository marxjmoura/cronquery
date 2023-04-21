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

public sealed class ExpressionWithDigitTest
{
    [Fact]
    public void ShouldGetNextMinuteAfterLastSecond()
    {
        var expression = new CronExpression("30 * * * * *");
        var current = new DateTime(2018, 12, 30, 08, 30, 30);
        var expected = new DateTime(2018, 12, 30, 08, 31, 30);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextHourAfterLastMinute()
    {
        var expression = new CronExpression("* 30 * * * *");
        var current = new DateTime(2018, 12, 30, 08, 30, 59);
        var expected = new DateTime(2018, 12, 30, 09, 30, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextDayAfterLastHour()
    {
        var expression = new CronExpression("* * 8 * * *");
        var current = new DateTime(2018, 12, 30, 23, 59, 59);
        var expected = new DateTime(2018, 12, 31, 08, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextMonthAfterLastDay()
    {
        var expression = new CronExpression("* * * 15 * *");
        var current = new DateTime(2018, 12, 31, 23, 59, 59);
        var expected = new DateTime(2019, 01, 15, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextMonthAfterLastDayInMonth()
    {
        var expression = new CronExpression("* * * 31 * *");
        var current = new DateTime(2019, 01, 31, 23, 59, 59);
        var expected = new DateTime(2019, 03, 31, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextYearAfterLastMonth()
    {
        var expression = new CronExpression("* * * * 6 *");
        var current = new DateTime(2018, 12, 31, 23, 59, 59);
        var expected = new DateTime(2019, 06, 01, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextDayOfWeek()
    {
        var expression = new CronExpression("* * * * * 3");
        var current = new DateTime(2018, 12, 28, 23, 59, 59);
        var expected = new DateTime(2019, 01, 02, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }
}
