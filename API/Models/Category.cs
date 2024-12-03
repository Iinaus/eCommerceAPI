using System;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class Category
{

    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; } = null;
    public required int UserId { get; set; }

    public virtual AppUser? User { get; set; }
    public virtual ICollection<Product> Products { get; set; } = [];
}
