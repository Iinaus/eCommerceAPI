

namespace API.Models;

public class Order
{
    public int Id { get; set; }
    public required DateTime CreatedDate { get; set; }
    public DateTime? ConfirmedDate { get; set; } = null;

    public DateTime? RemovedDate { get; set; } = null;
    public required string State { get; set; }
    public int CustomerId { get; set; }
    public virtual AppUser? Customer { get; set; }

    public int? HandlerId { get; set; } = null;

    public virtual AppUser? Handler { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = [];




}
