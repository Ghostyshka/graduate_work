using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;

namespace MailApp.Core.Interfaces
{
    public interface IDocumentService
    {
        public Task<byte[]> GetDocumentAsync(DocumentType documentType, MainPageViewModel vm, WriteMode mode, StorageFile file);
    }
}