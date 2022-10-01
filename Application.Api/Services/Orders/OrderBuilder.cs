using Application.Api.Data;
using Application.Api.Enums;
using Application.Api.Models;
using Application.Api.Models.Orders;
using Microsoft.EntityFrameworkCore;

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
        // TODO: product count -> how many
        var user = _context.Users.First(u => u.Id == id);

        var p = _context.Products.First(p => p.Id == product.Id);

        if (user.Products == null || p.Users == null)
        {
            user.Products = new List<Product>();
            p.Users = new List<User>();
        }
        
        user.Products.Add(p);
        p.Users.Add(user);
        
        _context.SaveChanges();
    }

    private Order BuildOrder(Guid id, EShippingType? shippingType)
    {
        var user = _context.Users.Include(x => x.Products).FirstOrDefault(u => u.Id == id);

        var order = new Order
        {
            User = user!,
            Products = user!.Products,
            ShippingType = (EShippingType) shippingType!
        };

        return order;
    }

    public Order GetOrder(Guid id, EShippingType type)
    {
        return BuildOrder(id, type);
    }

    public void RemoveProduct(Product product, Guid id)
    {
        var user = _context.Users.Include(x => x.Products).First(u => u.Id == id);

        var p = _context.Products.Include(x => x.Users).First(p => p.Id == product.Id);

        if (user.Products == null || p.Users == null) return;

        user.Products.Remove(p);
        p.Users.Remove(user);
        
        _context.SaveChanges();
    }
}