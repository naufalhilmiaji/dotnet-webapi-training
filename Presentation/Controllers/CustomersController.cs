using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Application.DTOs;

namespace NhjDotnetApi.Presentation.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomersController(ICustomerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _service.GetByIdAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create(CustomerRequest request)
    {
        await _service.CreateAsync(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Update(Guid id, CustomerRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
