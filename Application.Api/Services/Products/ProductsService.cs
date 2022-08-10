using Application.Api.Data;
using Application.Api.Models;

namespace Application.Api.Services.Products;

public class ProductsService : IProductsService
{
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

    public List<Product> GetProductsByName(string name)
    {
        try
        {
            return _context.Products.Where(p => p.Name.Contains(name)).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<Product>();
        }
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

    public (bool success, object content) UpdateProduct(Product product)
    {
        var productToUpdate = _context.Products.Find(product.Id);

        if (productToUpdate == null) return (false, new { message = $"Couldn't find product with id: {product.Id}" });

        productToUpdate.Name = product.Name;
        productToUpdate.Description = product.Description;
        productToUpdate.Price = product.Price;
        productToUpdate.Quantity = product.Quantity;

        _context.SaveChanges();
        return (true, new { message = $"Product with id {product.Id} has been updated" });
    }

    public (bool success, object content) RateProduct(int id, float rate)
    {
        
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product is null) return (false, new {message = $"Couldn't find product with id: {id}"});

        product.RateQuantity++;
        product.RateValue += rate;

        _context.SaveChanges();
        return (true, new { message = $"Product with id {product.Id} has been updated with rate of {product.AverageRate}" });
    }
}