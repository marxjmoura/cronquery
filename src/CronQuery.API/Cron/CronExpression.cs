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

namespace CronQuery.Cron;

using CronQuery.Extensions;

public sealed class CronExpression
{
    private readonly CronField _second = null!;
    private readonly CronField _minute = null!;
    private readonly CronField _hour = null!;
    private readonly CronField _day = null!;
    private readonly CronField _month = null!;
    private readonly CronField _dayOfWeek = null!;

    public CronExpression(string expression)
    {
        var subExpressions = (expression ?? string.Empty).ToUpper().Split(' ');
        var syntax = new CronSyntax(subExpressions);

        IsValid = syntax.IsValid();

        if (IsValid)
        {
            _second = new CronField(subExpressions[0], TimeUnit.Second);
            _minute = new CronField(subExpressions[1], TimeUnit.Minute);
            _hour = new CronField(subExpressions[2], TimeUnit.Hour);
            _day = new CronField(subExpressions[3], TimeUnit.Day);
            _month = new CronField(subExpressions[4], TimeUnit.Month);
            _dayOfWeek = new CronField(subExpressions[5], TimeUnit.DayOfWeek);
        }
    }

    public bool IsValid { get; }

    public DateTime Next(DateTime query)
    {
        if (!IsValid || IsUnreachableCondition())
        {
            return DateTime.MinValue;
        }

        var time = _second.Next(query);

        if (time <= query || !_minute.Matches(time))
        {
            time = _second.Reset(time);
            time = _minute.Next(time);
        }

        if (time <= query || !_hour.Matches(time))
        {
            time = _second.Reset(time);
            time = _minute.Reset(time);
            time = _hour.Next(time);
        }

        while (time <= query || !_day.Matches(time) || !_month.Matches(time) || !_dayOfWeek.Matches(time))
        {
            time = _second.Reset(time);
            time = _minute.Reset(time);
            time = _hour.Reset(time);

            time = _day.Next(time);

            if (!_dayOfWeek.Matches(time))
            {
                time = _dayOfWeek.Next(time);
            }

            if (time <= query || !_month.Matches(time))
            {
                time = _day.Reset(time);
                time = _month.Next(time);
            }

            if (time <= query)
            {
                time = _month.Reset(time);
                time = time.AddYears(1);
            }
        }

        return time;
    }

    private bool IsUnreachableCondition()
    {
        // E.g. * * * 15W * 0

        var hasW = _day.Values.Any(cron => cron.HasW);

        var onlyWeekend = !_dayOfWeek.Values
            .SelectMany(cron => cron.Values)
            .Any(value => 1.To(5).Contains(value));

        if (hasW && onlyWeekend) return true;

        // E.g. * * * 31 2,4,6 *

        var days = _day.Values.SelectMany(cron => cron.Values);
        var months = _month.Values.SelectMany(cron => cron.Values);

        var monthsNotReachGivenDays = days.Any() && !days.Any(day =>
            months.Any(month => DateTime.DaysInMonth(1970, month) >= day));

        return monthsNotReachGivenDays;
    }
}
