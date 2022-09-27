using Application.Api.Enums;
using Application.Api.Models.Orders;

namespace Application.Api.Models;

public class Invoice
{
    public int Id { get; set; }
    public Guid Number { get; set; }
    public string DateOfIssue { get; set; }
    public EPayingMethod PayingMethod { get; set; }

    public Order Order { get; set; }
}