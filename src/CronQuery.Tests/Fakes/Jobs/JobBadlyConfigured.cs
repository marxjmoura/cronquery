using System.Threading.Tasks;
using CronQuery.Mvc.Jobs;
using CronQuery.Mvc.Options;

namespace CronQuery.Tests.Fakes.Jobs
{
    public class JobBadlyConfigured : IJob
    {
        public Task RunAsync()
        {
            return Task.CompletedTask;
        }

        public static JobRunnerOptions Options
        {
            get
            {
                var options = new JobRunnerOptions();
                options.Running = true;
                options.Jobs.Add(new JobOptions
                {
                    Running = true,
                    Name = nameof(JobBadlyConfigured),
                    Cron = "IN V A L I D"
                });

                return options;
            }
        }
    }
}
