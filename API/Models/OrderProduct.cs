

namespace API.Models;

public class OrderProduct
{

    public int OrderId { get; set; }

    public int ProductId { get; set; }
    public required int UnitCount { get; set; }
    public required int UnitPrice { get; set; }

    public virtual Order? Order { get; set; }
    public virtual Product? Product { get; set; }
}
