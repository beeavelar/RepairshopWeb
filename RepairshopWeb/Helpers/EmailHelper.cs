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
