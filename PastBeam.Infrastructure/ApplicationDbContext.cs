using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Entities;

namespace PastBeam.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
    }
}