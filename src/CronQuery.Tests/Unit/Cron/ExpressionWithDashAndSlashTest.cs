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

public sealed class ExpressionWithDashAndSlashTest
{
    [Fact]
    public void ShouldGetNextSecond()
    {
        var expression = new CronExpression("20-50/10 * * * * *");
        var current = new DateTime(2018, 12, 30, 08, 15, 30);
        var expected = new DateTime(2018, 12, 30, 08, 15, 40);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextMinuteAfterLastSecond()
    {
        var expression = new CronExpression("20-50/10 * * * * *");
        var current = new DateTime(2018, 12, 30, 08, 15, 50);
        var expected = new DateTime(2018, 12, 30, 08, 16, 20);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextMinute()
    {
        var expression = new CronExpression("* 20-50/10 * * * *");
        var current = new DateTime(2018, 12, 30, 08, 30, 59);
        var expected = new DateTime(2018, 12, 30, 08, 40, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextHourAfterLastMinute()
    {
        var expression = new CronExpression("* 20-50/10 * * * *");
        var current = new DateTime(2018, 12, 30, 08, 50, 59);
        var expected = new DateTime(2018, 12, 30, 09, 20, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextHour()
    {
        var expression = new CronExpression("* * 8-18/2 * * *");
        var current = new DateTime(2018, 12, 30, 08, 59, 59);
        var expected = new DateTime(2018, 12, 30, 10, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextDayAfterLastHour()
    {
        var expression = new CronExpression("* * 8-18/2 * * *");
        var current = new DateTime(2018, 12, 30, 18, 59, 59);
        var expected = new DateTime(2018, 12, 31, 08, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextDay()
    {
        var expression = new CronExpression("* * * 5-25/5 * *");
        var current = new DateTime(2018, 12, 10, 23, 59, 59);
        var expected = new DateTime(2018, 12, 15, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextMonthAfterLastDay()
    {
        var expression = new CronExpression("* * * 5-25/5 * *");
        var current = new DateTime(2018, 12, 25, 23, 59, 59);
        var expected = new DateTime(2019, 01, 05, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextMonth()
    {
        var expression = new CronExpression("* * * * 2-12/2 *");
        var current = new DateTime(2018, 12, 31, 23, 59, 59);
        var expected = new DateTime(2019, 02, 01, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }

    [Fact]
    public void ShouldGetNextDayOfWeek()
    {
        var expression = new CronExpression("* * * * * 1-5/2");
        var current = new DateTime(2018, 12, 28, 23, 59, 59);
        var expected = new DateTime(2018, 12, 31, 00, 00, 00);

        Assert.Equal(expected, expression.Next(current));
    }
}
