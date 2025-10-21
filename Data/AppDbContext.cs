using Microsoft.EntityFrameworkCore;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;

public class AppDbContext(IConfiguration config) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = config.GetConnectionString("Database");
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
    }

    // https://www.jetbrains.com/help/rider/EntityFramework.ModelValidation.UnlimitedStringLength.html
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(20);
    // }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
}