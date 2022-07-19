using Application.Api.Models.Orders;
using Application.Api.Services.Products;

namespace Application.Api.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IProductsService _productsService;
    private readonly IOrderBuilder _orderBuilder;

    public OrderService(IProductsService productsService, IOrderBuilder orderBuilder)
    {
        _productsService = productsService;
        _orderBuilder = orderBuilder;
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
}