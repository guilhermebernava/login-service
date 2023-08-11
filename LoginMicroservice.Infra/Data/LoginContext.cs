using LoginMicroservice.Domain.Entities;
using LoginMicroservice.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LoginMicroservice.Infra.Data;

public class LoginContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public LoginContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
