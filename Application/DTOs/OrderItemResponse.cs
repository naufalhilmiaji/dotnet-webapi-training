namespace NhjDotnetApi.Presentation.Models;

public class OrderItemResponse
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal => Quantity * Price;
}
