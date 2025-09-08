using Microsoft.EntityFrameworkCore;
using BlazorApp.Domain.Entities; // Changed from BlazorApp.Domain

namespace BlazorApp.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}