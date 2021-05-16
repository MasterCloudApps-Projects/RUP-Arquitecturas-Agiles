using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ShareThings.Services.External.Sendgrid
{
    public class EmailSenderSendgrid : IEmailSender
    {
        private readonly SendGridEmailSenderOptions _options;

        public EmailSenderSendgrid(IOptions<SendGridEmailSenderOptions> options)
        {
            this._options = options.Value;
        }        

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Execute(_options.ApiKey, subject, htmlMessage, email);
        }

        private async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress(_options.SenderEmail);
            EmailAddress to = new EmailAddress(email);
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);
            Response response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Fail sending mail");
            return response;
        }
    }
}