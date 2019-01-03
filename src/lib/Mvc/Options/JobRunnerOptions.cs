using System.Collections.Generic;

namespace CronQuery.Mvc.Options
{
    internal class JobRunnerOptions
    {
        public bool Running { get; set; }
        public string Timezone { get; set; }
        public ICollection<JobOptions> Jobs { get; private set; } = new List<JobOptions>();
    }
}
