using System.Globalization;
using Application.Api.Data;
using Application.Api.Models;
using Application.Api.Services.Newsletter;
using Microsoft.EntityFrameworkCore;

namespace Application.Api.Services.Products;

public class ProductsService : IProductsService
{
    private readonly ApplicationContext _context;
    private readonly INewsletterService _newsletterService;

    public ProductsService(ApplicationContext context, INewsletterService newsletterService)
    {
        _context = context;
        _newsletterService = newsletterService;
    }
    
    public List<Product> GetAllProducts()
    {
        var products = _context.Products.Include(x => x.Comments).ToList();
        
        foreach (var product in products)
        {
            foreach (var productComment in product.Comments)
            {
                var comment = _context.Comments.Include(x => x.Author).First(x => x.Id == productComment.Id);
                productComment.Author = comment.Author;
            }
        }

        return products;
    }

    public Product GetProduct(int id)
    {
        var product = _context.Products.Include(x => x.Comments).FirstOrDefault(x => x.Id == id) ?? new Product();
        foreach (var productComment in product.Comments)
        {
            var comment = _context.Comments.Include(x => x.Author).First(x => x.Id == productComment.Id);
            productComment.Author = comment.Author;
        }
        return product;
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
        
        _newsletterService.SendNotificationToAll(product);
        
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

    public (bool success, object content) CommentProduct(int productId, Guid userId, string comment)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product is null) return (false, new {message = $"Couldn't find product with id: {productId}"});
        
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user is null) return (false, new {message = $"Couldn't find user with id: {userId}"});

        user.Comments ??= new List<Comment>();
        product.Comments ??= new List<Comment>();

        var newComment = new Comment
        {
            Content = comment,
            Author = user,
            Product = product,
            TimeCreated = DateTime.Now.ToString(CultureInfo.InvariantCulture)
        };
        
        user.Comments.Add(newComment);
        product.Comments.Add(newComment);
        _context.Comments.Add(newComment);

        _context.SaveChanges();
        
        return (true, new {message = "Added new comment"});
    }
}