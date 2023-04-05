using bist.Models;
using Microsoft.EntityFrameworkCore;

namespace bist.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Admin> admins { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Hisse> hisses { get; set; }
        public DbSet<Price> prices { get; set; }

    }

}

