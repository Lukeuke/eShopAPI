using Application.SMTP.Dtos;
using Application.SMTP.Enums;
using Application.SMTP.Services;

namespace Application.SMTP;

public class MailBuilder : IMailBuilder
{
    private ISmtpService _service = null!;
    
    public ISmtpService Build(MailType type, RequestDto? request)
    {
        switch (type)
        {
            case MailType.Gmail:
                if (request != null) _service = new GmailService(request);
                _service = new GmailService();
                break;
            default:
                break;
        }
        
        return _service;
    }
}

public interface IMailBuilder
{
    public ISmtpService Build(MailType type, RequestDto? request);
}