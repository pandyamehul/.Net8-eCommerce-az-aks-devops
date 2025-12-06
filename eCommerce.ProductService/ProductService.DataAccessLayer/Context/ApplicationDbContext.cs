using eCommerce.ProductsService.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.ProductsService.DataAccessLayer.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


    }
}