using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Api.Enums;
using Application.Api.Models.Orders;

namespace Application.Api.Models;

public class Invoice
{
    [Key]
    public Guid Number { get; set; }
    public string DateOfIssue { get; set; } = null!;
    public EPayingMethod PayingMethod { get; set; }
    public string Vendor => Order.User.Name;
    public decimal Total => Order.PriceSum();
    
    public Order Order { get; set; }
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }
}