using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace tests.Mvc.Fakes
{
    public class LoggerFake : ILogger
    {
        public ICollection<string> Messages { get; private set; } = new List<string>();

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            Messages.Add(formatter(state, exception));
        }
    }
}
