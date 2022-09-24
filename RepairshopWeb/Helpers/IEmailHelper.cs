using System.Threading.Tasks;

namespace RepairshopWeb.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmail(string email, string subject);
    }
}
