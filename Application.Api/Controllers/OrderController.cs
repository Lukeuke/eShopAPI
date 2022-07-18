using Application.Api.Models.Orders;
using Application.Api.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
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
        return Ok("Product has been added to your cart");
    }
}