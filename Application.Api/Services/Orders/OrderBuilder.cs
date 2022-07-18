using Application.Api.Data;
using Application.Api.Models;
using Application.Api.Models.Orders;

namespace Application.Api.Services.Orders;

public class OrderBuilder : IOrderBuilder
{
    public OrderBuilder(ApplicationContext context)
    {
        _context = context;
    }
    
    private readonly ApplicationContext _context;

    public void AddProduct(Product product, Guid id)
    {
        /*var user = _context.Users.First(u => u.Id == id);
        
        if (user.Products != null) user.Products.Add(product);

        _context.SaveChanges();*/
    }

    private Order BuildOrder(Guid id)
    {
        /*var user = _context.Users.First(u => u.Id == id);
        var products = user.Products.ToList();

        var order = new Order
        {
            Products = products
        };

        return order;*/
        return new Order();
    }

    public Order GetOrder(Guid id)
    {
        return BuildOrder(id);
    }
}