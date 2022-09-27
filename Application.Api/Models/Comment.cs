namespace Application.Api.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public User Author { get; set; }
    public Product Product { get; set; }
    public string TimeCreated { get; set; }
    public int LikesCount { get; set; }
}