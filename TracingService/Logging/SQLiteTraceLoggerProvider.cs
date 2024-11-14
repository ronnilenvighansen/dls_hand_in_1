namespace TracingService.Logging
{
    public class SQLiteTraceLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;

        public SQLiteTraceLoggerProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName) => new SQLiteTraceLogger(_connectionString);

        public void Dispose() { }
    }
}
