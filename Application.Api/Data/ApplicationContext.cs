using Application.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Api.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();

        modelBuilder.Entity<User>(eb =>
        {
            eb.HasMany(u => u.Products).WithMany(p => p.Users);
        });

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}