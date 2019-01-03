using System.Threading.Tasks;
using tests.Mvc.Jobs;
using tests.Mvc.Fakes;
using Xunit;

namespace tests.Mvc
{
    public class JobRunnerTest
    {
        [Fact]
        public async Task ShouldRun()
        {
            var server = new ServerFake();

            await Task.Delay(1500); // Waiting for jobs

            Assert.True(server.Job<JobSuccessful>().Executed);
            Assert.Contains(server.Logger.Messages, message => message == $"Job {nameof(JobWithError)} failed during running.");
        }
    }
}
