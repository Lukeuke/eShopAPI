using Application.Api.Data;
using Application.Api.Models;

namespace Application.Api.Services.Products;

public class ProductsService : IProductsService
{
    // TODO: Make this Authorized only
    private readonly ApplicationContext _context;

    public ProductsService(ApplicationContext context)
    {
        _context = context;
    }
    
    public List<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProduct(int id)
    {
        var result = _context.Products.Find(id) ?? new Product();
        return result;
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void RemoveProduct(int id)
    {
        var product = _context.Products.Find(id) ?? new Product();
        _context.Products.Remove(product);
        _context.SaveChanges();
    }
}