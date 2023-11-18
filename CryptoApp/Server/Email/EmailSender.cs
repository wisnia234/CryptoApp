using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using CryptoApp.Server.AppOptions;
using Microsoft.Extensions.Options;

namespace CryptoApp.Server.Email;

internal class EmailSender : IEmailSender
{
    private readonly EmailOptions _emailOptions;
    public EmailSender(IOptions<EmailOptions> options)
    {
        _emailOptions = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        string emailSender = _emailOptions.FromAddress;
        string password = _emailOptions.FromPassword;

        SmtpClient smtp = new()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(emailSender, password)
        };

        using MailMessage message = new(emailSender, email)
        {
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        await smtp.SendMailAsync(message);
    }
}
