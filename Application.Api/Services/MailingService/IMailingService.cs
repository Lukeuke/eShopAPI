using Application.SMTP.Dtos;

namespace Application.Api.Services.MailingService;

public interface IMailingService
{
    (bool success, object content) SendMail(RequestDto request);
}