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
