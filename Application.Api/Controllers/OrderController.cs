using Application.Api.Enums;
using Application.Api.Models.Orders;
using Application.Api.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost("get")]
    public Order GetOrder(Guid userId)
    {
        return _orderService.GetOrder(userId);
    }

    [HttpPost("add")]
    public IActionResult AddProduct(int id, Guid userId)
    {
        _orderService.AddProduct(id, userId);
        return Ok(new { Message = "Product has been added to your cart" });
    }
    
    [HttpDelete("remove")]
    public IActionResult RemoveProduct(int id, Guid userId)
    {
        _orderService.RemoveProduct(id, userId);
        return Ok(new { Message = "Product has been removed from your cart" });
    }

    [HttpDelete("finish")]
    public IActionResult FinishOrder(Guid id, EPayingMethod payingMethod, EShippingType shippingType)
    {
        var (success, content) = _orderService.FinishOrder(id, payingMethod, shippingType);

        if (!success) return BadRequest(content);
        
        return Ok(content);
    }
    
    [HttpGet("completedOrders")]
    public IActionResult GetCompletedOrders(Guid userId)
    {
        var (success, content) = _orderService.GetCompletedOrder(userId);
        
        if (!success) return BadRequest(content);
        
        return Ok(content);
    }
    
    [HttpGet("completedOrder")]
    public IActionResult GetCompletedOrder(Guid orderId)
    {
        var (success, content) = _orderService.GetCompletedOrderById(orderId);
        
        if (!success) return BadRequest(content);
        
        return Ok(content);
    }
}