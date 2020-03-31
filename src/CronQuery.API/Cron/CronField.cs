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
using System.Collections.Generic;
using System.Linq;
using CronQuery.Extensions;

namespace CronQuery.Cron
{
    internal sealed class CronField
    {
        private readonly TimeUnit _timeUnit;

        public CronField(string expression, TimeUnit timeUnit)
        {
            _timeUnit = timeUnit;

            Values = expression.Split(',')
                .Select(value => new CronValue(value, timeUnit))
                .ToList();
        }

        public IEnumerable<CronValue> Values { get; }

        public bool Matches(DateTime dateTime)
        {
            if (Values.Any(value => value.HasW))
            {
                return dateTime == NearestWeekday(dateTime);
            }

            if (Values.Any(value => value.HasL))
            {
                return dateTime == LastDayInMonth(dateTime);
            }

            if (Values.Any(value => value.HasHash))
            {
                return dateTime == DayOfWeekOccurence(dateTime);
            }

            return Values.SelectMany(cron => cron.Values).Contains(dateTime.Get(_timeUnit));
        }

        public DateTime Next(DateTime dateTime)
        {
            if (Values.Any(value => value.HasW))
            {
                return NearestWeekday(dateTime);
            }

            if (Values.Any(value => value.HasL))
            {
                return LastDayInMonth(dateTime);
            }

            if (Values.Any(value => value.HasHash))
            {
                return DayOfWeekOccurence(dateTime);
            }

            return ListValue(dateTime);
        }

        public DateTime Reset(DateTime dateTime)
        {
            var values = Values.SelectMany(cron => cron.Values);

            if (!values.Any())
            {
                return dateTime;
            }

            return dateTime.Set(_timeUnit, values.Min());
        }

        private DateTime ListValue(DateTime dateTime)
        {
            var values = Values.SelectMany(cron => cron.Values);

            var next = values
                .Where(value => value > dateTime.Get(_timeUnit))
                .Select(value => (int?)value)
                .FirstOrDefault();

            next = next ?? values.Min();

            if (_timeUnit.IsDay && next.Value > DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
            {
                dateTime = dateTime.AddMonths(1);
                next = values.Min();
            }

            if (_timeUnit.IsMonth)
            {
                var daysInMonth = DateTime.DaysInMonth(dateTime.Year, next.Value);

                if (dateTime.Day > daysInMonth)
                {
                    dateTime = dateTime.Set(day: daysInMonth);
                }
            }

            return dateTime.Set(_timeUnit, next.Value);
        }

        private DateTime LastDayInMonth(DateTime dateTime)
        {
            var cron = Values.Single();

            if (_timeUnit.IsDay)
            {
                if (cron.IsL) // Expression is L
                {
                    return dateTime.Last();
                }

                return dateTime.Last(daysBefore: cron.Values.Single()); // Expression is L-[1-31]
            }

            return dateTime.Last().Previous((DayOfWeek)cron.Values.Single()); // Expression is L or [0-6]L
        }

        private DateTime NearestWeekday(DateTime dateTime)
        {
            var cron = Values.Single();

            var day = cron.HasL ?
                DateTime.DaysInMonth(dateTime.Year, dateTime.Month) : // Expression is LW
                cron.Values.Single(); // Expression is [1-31]W

            if (day > DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
            {
                dateTime = dateTime.AddMonths(1);
            }

            return dateTime.Set(day: day).NearestWeekday();
        }

        private DateTime DayOfWeekOccurence(DateTime dateTime)
        {
            var cron = Values.Single();
            var dayOfWeek = (DayOfWeek)cron.Values.Single();

            return dateTime.Next(dayOfWeek, cron.Nth);
        }
    }
}
