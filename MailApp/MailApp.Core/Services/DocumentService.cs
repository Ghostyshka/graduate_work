using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailApp.Core.Interfaces;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;
using SaveOptions = Aspose.Words.Saving.SaveOptions;

namespace MailApp.Core.Services
{
    public class DocumentService : IDocumetService
    {
        public async Task<byte[]> GetDocumentAsync(DocumentType documentType, MainPageViewModel vm)
        {
            if (!vm.EmailDatas.Any())
            {
                return Array.Empty<byte>();
            }
            switch (documentType)
            {
                case DocumentType.Doc:
                    return await GetDocAsync(
                        vm.EmailDatas.Where(x => string.Equals(x.Subject, documentType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            .Select(x => new DocModel(x.Body)), false);
                case DocumentType.Pdf:
                    return await GetDocAsync(
                        vm.EmailDatas.Where(x => string.Equals(x.Subject, documentType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            .Select(x => new PdfModel(x.Body)), true);
                case DocumentType.Pptx:
                    return Array.Empty<byte>();
                default:
                    return Array.Empty<byte>();
            }
        }
        private async Task<byte[]> GetDocAsync(IEnumerable<DocModel> payloads, bool isPdf)
        {
            var pathToDocument = AppDomain.CurrentDomain.BaseDirectory + "Assets\\DOC.docx";

            Aspose.Words.Document doc = new Aspose.Words.Document(pathToDocument);

            using var memStream = new MemoryStream();

            doc.Save(memStream, isPdf ? (SaveOptions)new Aspose.Words.Saving.PdfSaveOptions() : new Aspose.Words.Saving.DocSaveOptions());
            memStream.Seek(0, SeekOrigin.Begin);

            var bytes = new byte[memStream.Length];
            _ = await memStream.ReadAsync(bytes, 0, (int)memStream.Length);

            return bytes;
        }

        //private Task<byte[]> GetPdfAsync(IEnumerable<string> payload)
        //{
        //    var pathToDocument = AppDomain.CurrentDomain.BaseDirectory + "Assets\\DOC.docx";

        //}

        //private Task<byte[]> GetPptxAsync(IEnumerable<string> payload)
        //{
        //    var pathToDocument = AppDomain.CurrentDomain.BaseDirectory + "Assets\\PPTX.pptx";

        //}


    }
}