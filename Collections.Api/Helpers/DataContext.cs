using System.Text.Json;

namespace Collections.Api.Helpers;

using Entities;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var host = _configuration["DB_HOST"] ?? "localhost";
        var port = _configuration["DB_PORT"] ?? "5432";
        var database = _configuration["DB_NAME"] ?? "pg";
        var username = _configuration["DB_USERNAME"] ?? "pg";
        var password = _configuration["DB_PASSWORD"] ?? "12345678";

        options.UseNpgsql(
            $"Host={host}; Port={port}; Database={database}; Username={username}; Password={password}; Include Error Detail=true");
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.Status).HasDefaultValue(true);
        modelBuilder.Entity<User>().Property(b => b.Admin).HasDefaultValue(true);

        modelBuilder.Entity<Collection>()
            .HasGeneratedTsVectorColumn(p => p.SimpleSearchVector, "simple", p => new { p.Name, p.Description })
            .HasIndex(p => p.SimpleSearchVector)
            .HasMethod("GIN");

        modelBuilder.Entity<Item>()
            .HasGeneratedTsVectorColumn(p => p.SimpleSearchVector, "simple", p => new { p.Name })
            .HasIndex(p => p.SimpleSearchVector)
            .HasMethod("GIN");

        modelBuilder.Entity<Field>()
            .HasGeneratedTsVectorColumn(p => p.SimpleSearchVector, "simple", p => new { p.Name })
            .HasIndex(p => p.SimpleSearchVector)
            .HasMethod("GIN");

        modelBuilder.Entity<StringValue>()
            .HasGeneratedTsVectorColumn(p => p.SimpleSearchVector, "simple", p => new { p.Value })
            .HasIndex(p => p.SimpleSearchVector)
            .HasMethod("GIN");

        modelBuilder.Entity<Tag>()
            .HasGeneratedTsVectorColumn(p => p.SimpleSearchVector, "simple", p => new { p.Name })
            .HasIndex(p => p.SimpleSearchVector)
            .HasMethod("GIN");

        modelBuilder.Entity<Topic>()
            .HasGeneratedTsVectorColumn(p => p.SimpleSearchVector, "simple", p => new { p.EnName, p.RuName })
            .HasIndex(p => p.SimpleSearchVector)
            .HasMethod("GIN");

        modelBuilder.Entity<Comment>()
            .HasGeneratedTsVectorColumn(p => p.SimpleSearchVector, "simple", p => new { p.Text })
            .HasIndex(p => p.SimpleSearchVector)
            .HasMethod("GIN");
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Collection> Collections => Set<Collection>();

    public DbSet<Topic> Topics => Set<Topic>();

    public DbSet<Item> Items => Set<Item>();

    public DbSet<Comment> Comments => Set<Comment>();

    public DbSet<Like> Likes => Set<Like>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Field> Fields => Set<Field>();

    public DbSet<IntValue> IntValues => Set<IntValue>();

    public DbSet<StringValue> StringValues => Set<StringValue>();

    public DbSet<DateTimeValue> DateTimeValues => Set<DateTimeValue>();

    public DbSet<BoolValue> BoolValues => Set<BoolValue>();
}
