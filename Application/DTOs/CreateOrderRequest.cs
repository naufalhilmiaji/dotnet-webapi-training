namespace NhjDotnetApi.Presentation.Models;


public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new();
}

