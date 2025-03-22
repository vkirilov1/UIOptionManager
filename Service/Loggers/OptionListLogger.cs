using Microsoft.Extensions.Logging;
using System.Text;

namespace Service.Loggers
{
    internal class OptionListLogger : ILogger
    {
        private readonly string _name;
        private static readonly string LogsFilePath;
        private static readonly object _fileLock = new();

        static OptionListLogger()
        {
            string baseDirectory = AppContext.BaseDirectory;

            LogsFilePath = Path.Combine(baseDirectory, "Logs", "OptionListLogs.txt");

            string? logsDirectory = Path.GetDirectoryName(LogsFilePath);
            if (string.IsNullOrEmpty(logsDirectory))
            {
                throw new InvalidOperationException("The log directory path is invalid.");
            }

            Directory.CreateDirectory(logsDirectory);
        }

        public OptionListLogger(string name)
        {
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // This logger does not support scopes.
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // This logger is always enabled
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);

            var logEntry = new StringBuilder();
            logEntry.Append($"[{DateTime.Now}]");
            logEntry.Append($"[{logLevel}]");
            logEntry.AppendFormat(" [{0}]", _name);
            logEntry.Append($" {message}");

            if (exception != null)
            {
                logEntry.Append($" Exception: {exception.Message}");
            }

            lock (_fileLock)
            {
                using (var writer = new StreamWriter(LogsFilePath, true))
                {
                    writer.WriteLine(logEntry.ToString());
                }
            }
        }
    }
}
