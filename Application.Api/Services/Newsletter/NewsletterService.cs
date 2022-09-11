using Application.Api.Data;
using Application.Api.Models;
using Application.Api.Services.MailingService;
using Application.SMTP.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Api.Services.Newsletter;

public class NewsletterService : INewsletterService
{
    private readonly ApplicationContext _context;
    private readonly IMailingService _mailingService;
    
    public NewsletterService(ApplicationContext context, IMailingService mailingService)
    {
        _context = context;
        _mailingService = mailingService;
    }
    
    public (bool success, object content) SendNotificationToAll(Product product)
    {
        var users = _context.Users.Include(x => x.Notifications).ToList();
        
        foreach (var user in users)
        {
            if (user.Notifications is null)
            {
                continue;
            }

            foreach (var notifications in user.Notifications)
            {
                if (notifications.Key != nameof(ENotificationType.ProductAdded) || notifications.Value == false) continue;

                var request = new RequestDto
                {
                    ToAddress = user.Email,
                    Body = NewsletterHelper.GenerateBody(product),
                    Subject = NewsletterHelper.GenerateSubject()
                };

                return _mailingService.SendMail(request);
            }
        }

        return (false, new { Message = "Couldn't send an Email"} );
    }

    public (bool success, object content) AddUserToNewsletter(Guid id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        
        if(user is null) return (false, new { Message = $"Couldn't find User with id: {id}"});
        user.Notifications ??= new List<Notifications<string, bool>>();

        var notifications = new Notifications<string, bool>
        {
            Key = nameof(ENotificationType.ProductAdded),
            Value = true
        };

        user.Notifications.Add(notifications);

        _context.SaveChanges();
        
        return (true, new { Message = "Added user to Newsletter"});
    }
}