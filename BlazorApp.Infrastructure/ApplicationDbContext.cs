using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Add your DbSets here. For example:
    // public DbSet<Product> Products { get; set; }
}