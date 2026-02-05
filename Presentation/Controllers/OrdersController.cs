using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Domain.Entities;
using NhjDotnetApi.Presentation.Models;
using NhjDotnetApi.Application.DTOs;

namespace NhjDotnetApi.Presentation.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    private Guid GetUserId() =>
    Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin() =>
        User.IsInRole("ADMIN");

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var orders = _orderService
            .GetAll(GetUserId(), IsAdmin())
            .Select(Map);

        return Ok(orders);
    }


    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetById(Guid id)
    {
        var order = _orderService.GetById(id, GetUserId(), IsAdmin());

        if (order == null)
            return NotFound();

        return Ok(Map(order));
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create(CreateOrderRequest request)
    {
        var userId = GetUserId();
        var order = _orderService.CreateOrder(request, userId);
        return Ok(Map(order));
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
    Guid id,
    UpdateOrderStatusRequest request)
    {
        var userId = GetUserId();
        var isAdmin = IsAdmin();

        var success = await _orderService.UpdateStatusAsync(
            id,
            request.NewStatus,
            userId,
            isAdmin
        );

        if (!success)
            return Forbid();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult Cancel(Guid id)
    {
        var success = _orderService.CancelOrder(
            id,
            GetUserId(),
            IsAdmin()
        );

        if (!success)
            return Forbid("Cannot cancel order.");

        return NoContent();
    }

    [HttpGet("export/excel")]
    public IActionResult ExportToExcel(
        [FromServices] IOrderExportService exportService)
    {
        var userId = GetUserId();
        var isAdmin = IsAdmin();

        var fileBytes = exportService.ExportOrdersToExcel(userId, isAdmin);

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "orders.xlsx"
        );
    }



    private static OrderResponse Map(Order o)
    {
        return new OrderResponse
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            CreatedAt = o.CreatedAt,

            Items = o.Items.Select(i => new OrderItemResponse
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }
}
