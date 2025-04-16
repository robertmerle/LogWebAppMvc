using LogWebAppMvc.Data;
using LogWebAppMvc.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LogWebAppMvc.Services
{
    public interface ILogService
    {
        Task<IEnumerable<LogModel>> GetAll();
    }

    public class LogService : ILogService
    {
        private readonly IConfiguration _configuration;

        public LogService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private string GetConnectionString()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");

            return connectionString;
        }

        public async Task<IEnumerable<LogModel>> GetAll()
        {
            var logs = new List<LogModel>();

            using var connection = new SqliteConnection(GetConnectionString());
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Message FROM Logs";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new LogModel(reader.GetInt32(0), reader.GetString(1)));
            }

            return logs;
        }
    }
}
