using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace LoggingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;
        private readonly string _connectionString;

        public LoggingController(ILogger<LoggingController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = "Data Source=logs.db";
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = new List<LogEntry>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqliteCommand("SELECT LogLevel, Message, Timestamp FROM Logs", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        logs.Add(new LogEntry
                        {
                            LogLevel = reader.GetString(0),
                            Message = reader.GetString(1),
                            Timestamp = reader.GetDateTime(2)
                        });
                    }
                }
            }
            return Ok(logs);
        }
    }
}