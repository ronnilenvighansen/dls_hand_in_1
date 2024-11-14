namespace LoggingService.Logging
{
    public class SQLiteLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;

        public SQLiteLoggerProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName) => new SQLiteLogger(_connectionString);

        public void Dispose() { }
    }
}
