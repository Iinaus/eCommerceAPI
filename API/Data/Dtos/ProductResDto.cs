using System;

namespace API.Data.Dtos;

public class ProductResDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int CategoryId { get; set; }
    public required int UnitPrice { get; set; }
}
