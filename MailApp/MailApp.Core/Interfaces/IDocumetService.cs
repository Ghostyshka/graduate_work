using System.Collections.Generic;
using System.Threading.Tasks;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;

namespace MailApp.Core.Interfaces
{
    public interface IDocumetService
    {
        public Task<byte[]> GetDocumentAsync(DocumentType documentType, MainPageViewModel vm);
    }
}