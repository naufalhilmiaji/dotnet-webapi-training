namespace NhjDotnetApi.Presentation.Models;

public class OrderItemRequest
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
