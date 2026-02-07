using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Presentation.Models;

public class OrderResponse
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }

    public List<OrderItemResponse> Items { get; set; } = new();

    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
