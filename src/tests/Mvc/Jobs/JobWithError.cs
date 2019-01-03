using System;
using System.Threading.Tasks;
using CronQuery.Mvc.Jobs;

namespace tests.Mvc.Jobs
{
    public class JobWithError : IJob, IDisposable
    {
        public void Dispose() { }

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
