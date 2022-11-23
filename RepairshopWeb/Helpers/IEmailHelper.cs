using System.IO;
using System.Threading.Tasks;

namespace RepairshopWeb.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmail(string email, string subject, string message);
        Task SendEmailWithAttachment(string email, string subject, string message, MemoryStream attachment);
    }
}
