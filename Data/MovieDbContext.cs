using Microsoft.EntityFrameworkCore;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;

public class MovieDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connection = "server=localhost; database=movies; user=root; password=;";
        
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
    }
    
    public DbSet<Movie> Movies { get; set; }
}