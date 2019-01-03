using Microsoft.Extensions.Logging;

namespace tests.Mvc.Fakes
{
    public class LoggerFactoryFake : ILoggerFactory
    {
        private static ILogger _logger;

        public void AddProvider(ILoggerProvider provider) { }

        public ILogger CreateLogger(string categoryName)
        {
            if (_logger == null)
            {
                _logger = new LoggerFake();
            }

            return _logger;
        }

        public void Dispose() { }
    }
}
