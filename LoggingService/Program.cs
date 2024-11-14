using Microsoft.Data.Sqlite;
using LoggingService.Logging;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=logs.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Logs (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            LogLevel TEXT,
            Message TEXT,
            Timestamp TEXT
        )";
    command.ExecuteNonQuery();
}

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddProvider(new SQLiteLoggerProvider(connectionString));
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
