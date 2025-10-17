using Microsoft.EntityFrameworkCore;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;

public class UserDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connection = "server=localhost; database=users; user=root; password=;";
        
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
    }
    
    public DbSet<User> Users { get; set; }
}