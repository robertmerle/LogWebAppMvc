using LogWebAppMvc.Services;
using Microsoft.Extensions.Configuration;
using System.Data.SQLite;

namespace LogWebAppMvcTest
{
    public class LogServiceTests
    {
        private readonly ILogService _logService;

        public LogServiceTests()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:DefaultConnection", "Data Source=test_logs.db" }
                })
                .Build();

            // Create the test DB and seed data
            if (!File.Exists("test_logs.db"))
            {
                using var connection = new SQLiteConnection("Data Source=test_logs.db");
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE Logs (Id INTEGER PRIMARY KEY, Message TEXT);
                    INSERT INTO Logs (Message) VALUES ('Test log 1');
                    INSERT INTO Logs (Message) VALUES ('Test log 2');
                ";
                command.ExecuteNonQuery();
            }

            _logService = new LogService(config);
        }

        [Fact]
        public async Task GetAllLogsAsync_Returns_Logs()
        {
            var logs = await _logService.GetAll();

            Assert.NotNull(logs);
            Assert.True(logs.Count() == 2);
            Assert.Contains(logs, l => l.Message == "Test log 1");
            Assert.Contains(logs, l => l.Message == "Test log 2");
        }
    }
}
