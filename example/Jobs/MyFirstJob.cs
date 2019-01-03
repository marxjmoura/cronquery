using System;
using System.Threading.Tasks;
using CronQuery.Mvc.Jobs;

namespace example.Jobs
{
    public class MyFirstJob : IJob
    {
        public Task RunAsync()
        {
            Console.WriteLine($"My first job: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            return Task.CompletedTask;
        }
    }
}
