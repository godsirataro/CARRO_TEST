using CARRO_API.Common.Utilities;
using CARRO_API.Entities;
using CARRO_API.Models;
using CARRO_API.Models.DefaultModels;
using CARRO_API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace CARRO_API.Services
{
    public class EmailService : IEmailService
    {
        private SmtpSettings _smtpSettings;
        private readonly ILogger _logger;

        public EmailService(
            SmtpSettings smtpSettings
            )
        {
            _smtpSettings = smtpSettings;

        }

        public async Task SendEmailSigupSuccess(User user)
        {
            try 
            {
                var emailMessage = new EmailMessage
                {
                    To = user.Email,
                    Cc = "", // Make Cc optional
                    Bcc = "", // Make Bcc optional
                    Subject = "Sigup Carro",
                    Message = "Sigup Success",
                };

                var messageToSend = CreateMimeMessage(emailMessage);

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.None);
                    client.Authenticate(_smtpSettings.Username, _smtpSettings.Password);
                    client.Send(messageToSend);
                    client.Disconnect(true);
                }
            }
            catch ( Exception ex )
            {
                throw new Exception("Email sending failed", ex);
            }
        }
        private MimeMessage CreateMimeMessage(EmailMessage emailMessage)
        {
            var mimeMessage = new MimeMessage();

            // Set the sender address
            mimeMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));

            // Set the recipient addresses
            foreach (var data in emailMessage.To.Split(";"))
            {
                if (!string.IsNullOrEmpty(data))
                {
                    mimeMessage.To.Add(new MailboxAddress("", data));
                }
            }

            // Set the CC addresses if any
            foreach (var data in emailMessage.Cc.Split(";"))
            {
                if (!string.IsNullOrEmpty(data))
                {
                    mimeMessage.Cc.Add(new MailboxAddress("", data));
                }
            }

            // Set the BCC addresses if any
            foreach (var data in emailMessage.Bcc.Split(";"))
            {
                if (!string.IsNullOrEmpty(data))
                {
                    mimeMessage.Bcc.Add(new MailboxAddress("", data));
                }
            }

            // Set the subject
            mimeMessage.Subject = emailMessage.Subject;

            // Set the body of the email
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<html><body><div>" + emailMessage.Message + "</div></body></html>";

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            return mimeMessage;
        }
    }
}
