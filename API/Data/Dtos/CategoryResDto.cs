using System;

namespace API.Data.Dtos;

public class CategoryResDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<ProductResDto> Products { get; set; } = new();
}
