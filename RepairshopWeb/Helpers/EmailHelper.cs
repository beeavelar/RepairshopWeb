﻿using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IO;
using System.Threading.Tasks;

namespace RepairshopWeb.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IConfiguration _configuration;

        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var nameFrom = _configuration["Email:NameFrom"];
            var from = _configuration["Email:From"];
            var smtp = _configuration["Email:Smtp"];
            var port = _configuration["Email:Port"];
            var password = _configuration["Email:Password"];

            var _email = new MimeMessage();
            _email.From.Add(new MailboxAddress(nameFrom, from));
            _email.To.Add(new MailboxAddress(email, email));
            _email.Subject = subject;

            _email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(_email);
                    client.Disconnect(true);
                }
            }
            catch (System.Exception)
            {

            }
        }

        public async Task SendEmailWithAttachment(string email, string subject, string message, MemoryStream attachment)
        {
            var nameFrom = _configuration["Email:NameFrom"];
            var from = _configuration["Email:From"];
            var smtp = _configuration["Email:Smtp"];
            var port = _configuration["Email:Port"];
            var password = _configuration["Email:Password"];

            var _email = new MimeMessage();
            _email.From.Add(new MailboxAddress(nameFrom, from));
            _email.To.Add(new MailboxAddress(email, email));
            _email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = message;

            byte[] pdfAsByteArray = attachment.ToArray();
            attachment.Close();
            builder.Attachments.Add("Bill.pdf", pdfAsByteArray, new ContentType("application", "pdf")); ;

            _email.Body = builder.ToMessageBody();

            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(_email);
                    client.Disconnect(true);
                }
            }
            catch (System.Exception)
            {

            }

        }
    }
}

