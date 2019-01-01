using System;
using System.Collections.Generic;
using System.Linq;
using CronQuery.Extensions;

namespace CronQuery.Cron
{
    internal class CronField
    {
        private readonly TimeUnit _timeUnit;

        public CronField(string expression, TimeUnit timeUnit)
        {
            _timeUnit = timeUnit;

            Values = expression.Split(',')
                .Select(value => new CronValue(value, timeUnit))
                .ToList();
        }

        public IEnumerable<CronValue> Values { get; private set; }

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

            if (_timeUnit.IsDayOfWeek)
            {
                return dateTime.Last().Previous((DayOfWeek)cron.Values.Single()); // Expression is L or [0-6]L
            }

            return dateTime;
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

            return dateTime.Next((DayOfWeek)cron.Values.Single(), cron.Nth);
        }
    }
}