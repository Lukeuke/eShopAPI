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
        var user = _context.Users.Include(x => x.Products).First(u => u.Id == id);

        if (user.Products == null) return (true, new {message = "No products were found in your cart"});
        
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

        _context.SaveChanges();

        return (true, new { message = "Your order was finished" });
    }
}