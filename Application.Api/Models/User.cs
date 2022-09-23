using System.ComponentModel.DataAnnotations;
using Application.Api.Authorization;
using Application.Api.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace Application.Api.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required] [MaxLength(15)]
    public string Username { get; set; }
    
    [Required]
    public string Name { get; set; }
    public string Surname { get; set; }
    [Required]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }

    public List<Product>? Products { get; set; }

    public List<ERoles> Roles { get; init; }

    public List<Comment>? Comments { get; set; }

    public List<Notifications<string, bool>>? Notifications { get; set; }
    public List<Order>? Orders { get; set; }
}