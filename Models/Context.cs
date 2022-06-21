#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace Products_And_Categories.Models;
public class Context : DbContext 
{ 
    public Context(DbContextOptions options) : base(options) { }
    public DbSet<Product> Products { get; set; } 
    public DbSet<Category> Categories { get; set; } 
    public DbSet <Association> ProductsAndCategories {get; set;}
}