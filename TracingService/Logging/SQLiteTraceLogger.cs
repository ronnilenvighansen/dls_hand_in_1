using Microsoft.Data.Sqlite;

namespace TracingService.Logging
{
    public class SQLiteTraceLogger : ILogger
    {
        private readonly string _connectionString;

        public SQLiteTraceLogger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var traceMessage = formatter(state, exception);
            var traceId = Guid.NewGuid().ToString();
            var timestamp = DateTime.UtcNow;

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Traces (TraceId, Message, Timestamp) 
                VALUES ($traceId, $message, $timestamp)";
            
            command.Parameters.AddWithValue("$traceId", traceId);
            command.Parameters.AddWithValue("$message", traceMessage);
            command.Parameters.AddWithValue("$timestamp", timestamp);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging trace to SQLite: {ex.Message}");
            }
        }
    }
}
