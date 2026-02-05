using NhjDotnetApi.Domain.Entities;

namespace NhjDotnetApi.Presentation.Models;

public class UpdateOrderStatusRequest
{
    public OrderStatus NewStatus { get; set; }
}
