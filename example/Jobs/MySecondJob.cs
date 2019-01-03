using System;
using System.Threading.Tasks;
using CronQuery.Mvc.Jobs;

namespace example.Jobs
{
    public class MySecondJob : IJob
    {
        public Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
