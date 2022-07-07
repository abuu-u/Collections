namespace Task4Back.Helpers;

using Microsoft.EntityFrameworkCore;
using Task4Back.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var Host = _configuration["DB_HOST"];
        var Port = _configuration["DB_PORT"];
        var Database = _configuration["DB_NAME"];
        var Username = _configuration["DB_USERNAME"];
        var Password = _configuration["DB_PASSWORD"];

        options.UseNpgsql($"Host={Host}; Port={Port}; Database={Database}; Username={Username}; Password={Password}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(b => b.Status)
            .HasDefaultValue(true);

        modelBuilder.Entity<User>()
            .Property(b => b.Created)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<User>()
            .Property(b => b.LastLogin)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }

    public DbSet<User> Users { get; set; }
}