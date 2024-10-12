using Microsoft.EntityFrameworkCore;
using MyPortalBackend.Models;

namespace MyPortalBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
