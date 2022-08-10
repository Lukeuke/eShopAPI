namespace Application.Api.Models;

public class Rate
{
    public int Id { get; set; }
    public float Average => Value / Id;
    public float Value { get; set; }
    public int Quantity { get; set; }

    public List<Product>? Products { get; set; }
}