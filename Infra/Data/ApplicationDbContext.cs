﻿namespace IWantApp.Infra.Data;

public class ApplicationDbContext: IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder Builder)
    {
        base.OnModelCreating(Builder);

        Builder.Ignore<Notification>();

        Builder.Entity<Product>()
            .Property(p => p.Name).IsRequired();
        Builder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(255);
        Builder.Entity<Product>()
            .Property(p => p.Price).HasColumnType("decimal(10,2)").IsRequired();

        Builder.Entity<Category>()
            .Property(p => p.Name).IsRequired();
        
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration) 
    {
        configuration.Properties<string>()
            .HaveMaxLength(100);
    }
}
