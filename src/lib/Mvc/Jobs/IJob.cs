using System.Threading.Tasks;

namespace CronQuery.Mvc.Jobs
{
    public interface IJob
    {
        Task RunAsync();
    }
}
