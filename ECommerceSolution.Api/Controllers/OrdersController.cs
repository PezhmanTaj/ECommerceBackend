using Microsoft.AspNetCore.Mvc;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(string id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDto)
    {
        var createdOrder = await _orderService.CreateOrderAsync(orderDto);
        if (createdOrder == null)
            return BadRequest("Order could not be created.");
        return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(string id, [FromBody] OrderDTO orderDto)
    {
        var result = await _orderService.UpdateOrderAsync(id, orderDto);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(string id)
    {
        var result = await _orderService.DeleteOrderAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredOrders([FromQuery] string? sellerId, [FromQuery] string? status, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orders = await _orderService.GetFilteredOrdersAsync(sellerId, status, startDate, endDate);
        return Ok(orders);
    }
}
