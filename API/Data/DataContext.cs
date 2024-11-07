using System;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    // Propertyn nimestä tulee
    // tietokantataulun nimi
    // Tässä siis Users

    // modeleista tehdään dbsetit
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderProduct> OrdersProducts { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>(entity =>
        {
            entity.HasIndex(c => c.Name).IsUnique();
        });

        builder.Entity<OrderProduct>()
            // ASP .net Core pohjautuu nimeämiskäytäntöihin
            // jokaisen modelin Id-propertysta tehdään oletuksena taulun perusavain
            // kun kyseessä on välitaulu, merkataan molempien päätaulujen viiteavaimet 
            // perusavaimeksi
            .HasKey(op => new
            {
                op.OrderId,
                op.ProductId
            });

        builder.Entity<Order>()
        .HasOne(o => o.Customer)
        .WithMany(c => c.OrdersOwned)
        .HasForeignKey(o => o.CustomerId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Order>()
        .HasOne(o => o.Handler)
        .WithMany(h => h.OrdersHandled)
        .HasForeignKey(o => o.HandlerId)
        .OnDelete(DeleteBehavior.NoAction);


        // relaatiot välitaulun ja päätetaulujen välillä
        builder.Entity<OrderProduct>()
        .HasOne(op => op.Order)
        .WithMany(o => o.OrderProducts)
        .HasForeignKey(op => op.OrderId);

        builder.Entity<OrderProduct>()
        .HasOne(op => op.Product)
        .WithMany(p => p.OrderProducts)
        .HasForeignKey(p => p.ProductId);


    }
}
