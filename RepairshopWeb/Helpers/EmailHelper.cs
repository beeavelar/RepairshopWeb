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
