using Application.Api.Models;

namespace Application.Api.Services.Newsletter;

public interface INewsletterService
{
    (bool success, object content) SendNotificationToAll(Product product);
    (bool success, object content) AddUserToNewsletter(Guid id);
}