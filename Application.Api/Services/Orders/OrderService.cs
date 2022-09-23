using Application.Api.Data;
using Application.Api.Models;
using Application.Api.Models.Orders;
using Application.Api.Services.MailingService;
using Application.Api.Services.Newsletter;
using Application.Api.Services.Products;
using Application.SMTP.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Api.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IProductsService _productsService;
    private readonly IOrderBuilder _orderBuilder;
    private readonly ApplicationContext _context;
    private readonly IMailingService _mailingService;

    public OrderService(IProductsService productsService, IOrderBuilder orderBuilder, ApplicationContext context, IMailingService mailingService)
    {
        _productsService = productsService;
        _orderBuilder = orderBuilder;
        _context = context;
        _mailingService = mailingService;
    }
    
    public void AddProduct(int id, Guid userId)
    {
        // Get product by id from Db
        var product = _productsService.GetProduct(id);
        _orderBuilder.AddProduct(product, userId);
    }

    public Order GetOrder(Guid id)
    {
        return _orderBuilder.GetOrder(id);
    }

    public void RemoveProduct(int id, Guid userId)
    {
        var product = _productsService.GetProduct(id);
        _orderBuilder.RemoveProduct(product, userId);
    }

    public (bool successs, object content) FinishOrder(Guid id)
    {
        var user = _context.Users.Include(x => x.Products).Include(o => o.Orders).First(u => u.Id == id);

        if (user.Products == null) return (true, new {message = "No products were found in your cart"});
        if (user.Orders == null) return (true, new {message = "No orders were found in your cart"});
        
        foreach (var product in user.Products)
        {
            product.Quantity--;
        }

        var order = GetOrder(id);
        order.Id = Guid.NewGuid();
        
        _mailingService.SendMail(new RequestDto
        {
            ToAddress = user.Email,
            Body = MailHelper.GenerateBody(order),
            Subject = MailHelper.GenerateSubject(order)
        });

        user.Products = new List<Product>();
        user.Orders.Add(order);
        _context.Orders.Add(order);
        
        _context.SaveChanges();

        return (true, new { message = "Your order was finished" });
    }

    public (bool successs, object content) GetCompletedOrder(Guid id)
    {
        var user = _context.Users.Include(x => x.Orders)
            .Include(x => x.Products)
            .FirstOrDefault(x => x.Id == id);
        
        if (user is null) return (true, new {message = $"Couldn't find user with id of {id}"});
        if (user.Orders is null) return (true, new {message = $"Couldn't find orders from user of id {id}"});

        return (true, user.Orders);
    }

    public (bool successs, object content) GetCompletedOrderById(Guid orderId)
    {
        var order = _context.Orders.Include(x => x.User)
            .Include(x => x.Products)
            .FirstOrDefault(x => x.Id == orderId);
        
        if (order is null) return (true, new {message = $"Couldn't find orders with id of {orderId}"});

        return (true, order);
    }
}