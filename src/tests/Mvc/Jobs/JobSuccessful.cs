using System;
using System.Threading.Tasks;
using CronQuery.Mvc.Jobs;

namespace tests.Mvc.Jobs
{
    public class JobSuccessful : IJob, IDisposable
    {
        public bool Executed { get; private set; }

        public void Dispose() { }

        public Task RunAsync()
        {
            Executed = true;

            return Task.CompletedTask;
        }
    }
}
