using System;

namespace API.Data.Dtos;

public class OrderResDto
{
    public int Id { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required string State { get; set; }
    public int CustomerId { get; set; }

    public List<OrderProductResDto> OrderProducts { get; set; } = new();

}
