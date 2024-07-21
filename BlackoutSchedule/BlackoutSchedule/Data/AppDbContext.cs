using BlackoutSchedule.Models;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace BlackoutSchedule.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Groups> Groups { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Schedules> Schedules { get; set; }

    }
}   
