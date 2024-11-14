using Microsoft.Data.Sqlite;
using TracingService.Logging;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=traces.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Traces (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            TraceId TEXT,
            Message TEXT,
            Timestamp TEXT
        )";
    command.ExecuteNonQuery();
}

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddProvider(new SQLiteTraceLoggerProvider(connectionString));
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
