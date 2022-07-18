using System.ComponentModel.DataAnnotations;

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
}