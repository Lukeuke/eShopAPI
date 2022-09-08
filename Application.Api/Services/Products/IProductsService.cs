using Application.Api.Models;

namespace Application.Api.Services.Products;

public interface IProductsService
{
    List<Product> GetAllProducts();
    Product GetProduct(int id); 
    List<Product> GetProductsByName(string name);
    void AddProduct(Product product);
    void RemoveProduct(int id);
    (bool success, object content) UpdateProduct(Product product);
    (bool success, object content) RateProduct(int id, float rate);
    (bool success, object content) CommentProduct(int productId, Guid userId, string comment);
}