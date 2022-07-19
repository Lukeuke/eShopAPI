using Application.Api.Models;
using Application.Api.Models.Orders;

namespace Application.Api.Services.Orders;

public interface IOrderBuilder
{
    void AddProduct(Product product, Guid id);
     Order GetOrder(Guid id);
     void RemoveProduct(Product product, Guid id);
}