namespace CronQuery.Cron
{
    internal class TimeUnit
    {
        private TimeUnit(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
        public bool IsSecond { get; private set; }
        public bool IsMinute { get; private set; }
        public bool IsHour { get; private set; }
        public bool IsDay { get; private set; }
        public bool IsMonth { get; private set; }
        public bool IsDayOfWeek { get; private set; }

        public static TimeUnit Second => new TimeUnit(0, 59) { IsSecond = true };
        public static TimeUnit Minute => new TimeUnit(0, 59) { IsMinute = true };
        public static TimeUnit Hour => new TimeUnit(0, 23) { IsHour = true };
        public static TimeUnit Day => new TimeUnit(1, 31) { IsDay = true };
        public static TimeUnit Month => new TimeUnit(1, 12) { IsMonth = true };
        public static TimeUnit DayOfWeek => new TimeUnit(0, 6) { IsDayOfWeek = true };
    }
}
