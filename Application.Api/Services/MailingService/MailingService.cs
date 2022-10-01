using Application.SMTP;
using Application.SMTP.Dtos;
using Application.SMTP.Enums;

namespace Application.Api.Services.MailingService;

public class MailingService : IMailingService
{
    private readonly IMailBuilder _mailBuilder;

    public MailingService(IMailBuilder mailBuilder)
    {
        _mailBuilder = mailBuilder;
    }
    
    public (bool success, object content) SendMail(RequestDto request)
    {
        var mail = _mailBuilder.Build(MailType.Gmail, null, false, null);
        
        var (success, content) = mail.SendEmail(request);

        return (success, content);
    }
    
    public (bool success, object content) SendMailWithAttachment(RequestDto request, string path)
    {
        var mail = _mailBuilder.Build(MailType.Gmail, null, true, path);
        
        var (success, content) = mail.SendEmailWithAttachment(request, path);

        return (success, content);
    }
}