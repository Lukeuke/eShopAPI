using Application.Api.Models;
using Application.Api.Models.Orders;
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
            eb.HasMany(x => x.Orders);
        });
        
        modelBuilder.Entity<Order>(eb =>
        {
            eb.HasMany(x => x.Products);
            eb.HasOne(x => x.User);
        });

        modelBuilder.Entity<Product>(eb =>
        {
            eb.HasMany(x => x.Orders);
        });
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Order> Orders { get; set; }
}