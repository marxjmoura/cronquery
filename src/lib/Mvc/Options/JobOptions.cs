namespace CronQuery.Mvc.Options
{
    internal class JobOptions
    {
        public bool Running { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
    }
}
