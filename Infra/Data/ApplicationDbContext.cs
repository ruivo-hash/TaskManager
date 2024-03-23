using Microsoft.EntityFrameworkCore;
using TaskManager.Domain;

namespace TaskManager.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
    }
}
