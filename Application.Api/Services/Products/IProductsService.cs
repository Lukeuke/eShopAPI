using Application.Api.Models;

namespace Application.Api.Services.Products;

public interface IProductsService
{
    List<Product> GetAllProducts();
    Product GetProduct(int id); 
    void AddProduct(Product product);
    void RemoveProduct(int id);
}