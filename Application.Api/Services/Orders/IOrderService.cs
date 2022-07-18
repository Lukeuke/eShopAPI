using Application.Api.Models.Orders;

namespace Application.Api.Services.Orders;

public interface IOrderService
{
    void AddProduct(int id, Guid userId);
    Order GetOrder(Guid id);
}