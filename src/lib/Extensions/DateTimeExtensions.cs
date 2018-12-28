using System;
using CronQuery.Cron;

namespace CronQuery.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime Last(this DateTime dateTime, int daysBefore = 0)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            var day = daysInMonth > daysBefore ? daysInMonth - daysBefore : 1;

            return dateTime.Set(day: day);
        }

        public static DateTime NearestWeekday(this DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return dateTime.Day == 1 ?
                    dateTime.Next(DayOfWeek.Monday) :
                    dateTime.Previous(DayOfWeek.Friday);
            }

            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                var lastDay = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

                return dateTime.Day == lastDay ?
                    dateTime.Previous(DayOfWeek.Friday) :
                    dateTime.Next(DayOfWeek.Monday);
            }

            return dateTime;
        }

        public static DateTime Next(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            var diff = dayOfWeek - dateTime.DayOfWeek;

            if (diff <= 0)
            {
                diff += 7;
            }

            return dateTime.AddDays(diff);
        }

        public static DateTime Next(this DateTime dateTime, DayOfWeek dayOfWeek, int occurrence)
        {
            var firstDate = dateTime.Set(day: 1);

            dateTime = firstDate.AddDays((7 - ((int)firstDate.DayOfWeek - (int)dayOfWeek)) % 7);
            dateTime = dateTime.AddDays(7 * (occurrence - 1));

            return dateTime;
        }

        public static DateTime Previous(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            while (dateTime.DayOfWeek != dayOfWeek)
            {
                dateTime = dateTime.AddDays(-1);
            }

            return dateTime;
        }

        public static int Get(this DateTime dateTime, TimeUnit timeUnit)
        {
            if (timeUnit.IsSecond) return dateTime.Second;
            if (timeUnit.IsMinute) return dateTime.Minute;
            if (timeUnit.IsHour) return dateTime.Hour;
            if (timeUnit.IsDay) return dateTime.Day;
            if (timeUnit.IsMonth) return dateTime.Month;
            if (timeUnit.IsDayOfWeek) return (int)dateTime.DayOfWeek;

            throw new ArgumentException("Time unit does not specify granularity.", "timeUnit");
        }

        public static DateTime Set(this DateTime dateTime, TimeUnit timeUnit, int value)
        {
            if (timeUnit.IsSecond) return dateTime.Set(second: value);
            if (timeUnit.IsMinute) return dateTime.Set(minute: value);
            if (timeUnit.IsHour) return dateTime.Set(hour: value);
            if (timeUnit.IsDay) return dateTime.Set(day: value);
            if (timeUnit.IsMonth) return dateTime.Set(month: value);
            if (timeUnit.IsDayOfWeek) return dateTime.Next((DayOfWeek)value);

            throw new ArgumentException("Time unit does not specify granularity.", "timeUnit");
        }

        public static DateTime Set(this DateTime dateTime,
            int? year = null, int? month = null, int? day = null,
            int? hour = null, int? minute = null, int? second = null)
        {
            return new DateTime(
                year ?? dateTime.Year,
                month ?? dateTime.Month,
                day ?? dateTime.Day,
                hour ?? dateTime.Hour,
                minute ?? dateTime.Minute,
                second ?? dateTime.Second,
                dateTime.Millisecond,
                dateTime.Kind);
        }
    }
}
