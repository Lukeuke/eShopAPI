using System.ComponentModel.DataAnnotations;
using Application.Api.Enums;

namespace Application.Api.Models.Orders;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    public EShippingType ShippingType { get; set; }
    public List<Product>? Products { get; set; }
    public User User { get; set; }
    public Invoice Invoice { get; set; }
    
    public decimal PriceSum()
    {
        return Products!.Sum(product => product.Price);
    }
}