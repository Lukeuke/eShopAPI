using System.ComponentModel.DataAnnotations;

namespace Application.Api.Models;

public class Notifications<T, TU>
{
    [Key]
    public int Id { get; set; }
    public T Key { get; set; }
    public TU Value { get; set; }
}