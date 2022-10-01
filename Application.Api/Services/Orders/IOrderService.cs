using Application.Api.Enums;
using Application.Api.Models.Orders;

namespace Application.Api.Services.Orders;

public interface IOrderService
{
    void AddProduct(int id, Guid userId);
    Order GetOrder(Guid id);
    void RemoveProduct(int id, Guid userId);
    (bool successs, object content) FinishOrder(Guid id, EPayingMethod payingMethod, EShippingType shippingType);
    (bool successs, object content) GetCompletedOrder(Guid id);
    (bool successs, object content) GetCompletedOrderById(Guid orderId);
}