using System;

namespace API.Data.Dtos;

public class OrderProductResDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public required int UnitCount { get; set; }
    public required int UnitPrice { get; set; }
}
