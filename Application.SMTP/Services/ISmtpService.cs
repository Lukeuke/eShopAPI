using Application.SMTP.Dtos;

namespace Application.SMTP.Services;

public interface ISmtpService
{
    void SendEmail(RequestDto request);
}