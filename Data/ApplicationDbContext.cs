using Microsoft.EntityFrameworkCore;
using ActionNow.Models;

namespace ActionNow.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=application.db");
        }
    }
} 