using Microsoft.EntityFrameworkCore;
using Restuarnt.Services.Email.Models;
namespace Restuarnt.Services.Email.DbContexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<EmailLog>EmailLogs { get; set; }
    }
}
