namespace NhjDotnetApi.Presentation.SignalR;

public interface IOrderClient
{
    Task OrderStatusUpdated(Guid orderId, string status);
}
