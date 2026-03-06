using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Resend;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly IResend _resend;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;

        var apiKey = _configuration["Resend:ApiKey"];
        _resend = ResendClient.Create(apiKey);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var senderEmail = _configuration["Resend:SenderEmail"];

        var message = new EmailMessage();
        message.From = senderEmail;
        message.To.Add(email);
        message.Subject = subject;
        message.HtmlBody = htmlMessage;

        await _resend.EmailSendAsync(message);
    }
}
