using System;
using System.Linq;
using CronQuery.Extensions;

namespace CronQuery.Cron
{
    public class CronExpression
    {
        private readonly CronField _second;
        private readonly CronField _minute;
        private readonly CronField _hour;
        private readonly CronField _day;
        private readonly CronField _month;
        private readonly CronField _dayOfWeek;

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

        public bool IsValid { get; private set; }

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
                time = _minute.Reset(time);
                time = _hour.Next(time);
            }

            while (time <= query || !_day.Matches(time) || !_month.Matches(time) || !_dayOfWeek.Matches(time))
            {
                time = _second.Reset(time);
                time = _minute.Reset(time);
                time = _hour.Reset(time);

                time = _day.Next(time);

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
}
