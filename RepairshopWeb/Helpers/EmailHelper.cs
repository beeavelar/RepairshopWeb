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
            var from = new EmailAddress("cet69.repairshop@gmail.com", "RepairShop");
            var to = new EmailAddress(email);
            var content = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, message);

            await client.SendEmailAsync(msg);
        }
    }
}
