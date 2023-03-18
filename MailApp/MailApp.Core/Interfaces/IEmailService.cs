using System.Threading.Tasks;

namespace MailApp.Core.Interfaces
{
    public interface IEmailService
    {
        Task LoadEmails();
    }
}