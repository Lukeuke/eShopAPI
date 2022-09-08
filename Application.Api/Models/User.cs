using System.ComponentModel.DataAnnotations;
using Application.Api.Authorization;

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
    public string PasswordHash { get; set; }
    public string Salt { get; set; }

    public List<Product>? Products { get; set; }

    public List<ERoles> Roles { get; init; }

    public List<Comment>? Comments { get; set; }
}