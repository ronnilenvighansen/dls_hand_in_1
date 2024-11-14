using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace TracingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TracingController : ControllerBase
    {
        private readonly ILogger<TracingController> _logger;
        private readonly string _connectionString;

        public TracingController(ILogger<TracingController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = "Data Source=traces.db";
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTraces()
        {
            var traces = new List<TraceEntry>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqliteCommand("SELECT TraceId, Message, Timestamp FROM Traces", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        traces.Add(new TraceEntry
                        {
                            TraceId = reader.GetString(0),
                            Message = reader.GetString(1),
                            Timestamp = reader.GetDateTime(2)
                        });
                    }
                }
            }
            return Ok(traces);
        }

        [HttpPost]
        public IActionResult PostTrace([FromBody] TraceEntry traceEntry)
        {
            if (traceEntry == null || string.IsNullOrWhiteSpace(traceEntry.Message))
            {
                return BadRequest("Trace message cannot be null or empty.");
            }

            try
            {
                _logger.LogInformation($"Trace ID: {traceEntry.TraceId} - {traceEntry.Message}");
                return Ok("Trace message recorded.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while recording the trace message.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
