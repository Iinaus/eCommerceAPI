using System;

namespace API.Models;

public class AppUser
{
    // Muista, käyttää Id-propertysta juuri tällaista nimeä
    // asennamme EntityFrameWorkCore-riippvuuden Nugetista
    // EF Core tekee autom. Id-attribuutista tietokannan
    // taulun perusavaimen
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Role { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required byte[] HashedPassword { get; set; }

    public virtual ICollection<Order> OrdersOwned { get; set; } = [];
    public virtual ICollection<Order> OrdersHandled { get; set; } = [];
}