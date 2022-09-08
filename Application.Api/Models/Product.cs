using System.ComponentModel.DataAnnotations;

namespace Application.Api.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public float AverageRate => RateValue / RateQuantity;
    public float RateValue { get; set; }
    public int RateQuantity { get; set; }
    public List<User>? Users { get; set; }
    public List<Comment>? Comments { get; set; } 
}