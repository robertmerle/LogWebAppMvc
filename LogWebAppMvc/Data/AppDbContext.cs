using Microsoft.EntityFrameworkCore;

namespace LogWebAppMvc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Log> Logs { get; set; }
    }
}
