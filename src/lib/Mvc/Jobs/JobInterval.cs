using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CronQuery.Cron;

namespace CronQuery.Mvc.Jobs
{
    internal class JobInterval : IDisposable
    {
        private CronExpression _cron;
        private TimeZoneInfo _timezone;
        private Func<Task> _work;
        private IDisposable _subscription;

        public JobInterval(CronExpression cron, TimeZoneInfo timezone, Func<Task> work)
        {
            _cron = cron;
            _timezone = timezone;
            _work = work;
        }

        public void Dispose()
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
            }
        }

        public void Run()
        {
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timezone);
            var nextTime = _cron.Next(now);

            if (nextTime == DateTime.MinValue)
            {
                return;
            }

            var interval = nextTime - now;

            _subscription = Observable.Timer(interval)
                .Select(tick => Observable.FromAsync(_work))
                .Concat()
                .Subscribe(
                    onNext: tick => { /* noop */ },
                    onCompleted: Run
                );
        }
    }
}
