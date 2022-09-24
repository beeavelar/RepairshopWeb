using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace RepairshopWeb.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        public async Task SendEmail(string email, string subject, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("cet69.repairshop@gmail.com", "Repair Shop");
            var to = new EmailAddress(email);
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, htmlContent);

            await client.SendEmailAsync(msg);
        }
    }
}
