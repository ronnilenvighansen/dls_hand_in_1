using Microsoft.Data.Sqlite;

namespace LoggingService.Logging
{
    public class SQLiteLogger : ILogger
    {
        private readonly string _connectionString;

        public SQLiteLogger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Logs (LogLevel, Message, Timestamp) VALUES ($level, $message, $timestamp)";
            command.Parameters.AddWithValue("$level", logLevel.ToString());
            command.Parameters.AddWithValue("$message", message);
            command.Parameters.AddWithValue("$timestamp", DateTime.UtcNow);

            command.ExecuteNonQuery();

        }
    }
}
