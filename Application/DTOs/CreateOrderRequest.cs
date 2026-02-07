namespace NhjDotnetApi.Presentation.Models;


public class CreateOrderRequest
{
    public List<CreateOrderItem> Items { get; set; } = new();
}

public class CreateOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

