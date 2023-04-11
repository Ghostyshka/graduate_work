using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MailApp.Core.Interfaces;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;
using SautinSoft.Document;
using SautinSoft.Document.Tables;
using SaveOptions = SautinSoft.Document.SaveOptions;

namespace MailApp.Core.Services
{
    public class DocumentService : IDocumetService
    {
        private const string FirstSection = "Монографії";
        private const string SecondSection = "Навчальні посібники";
        public async Task<byte[]> GetDocumentAsync(DocumentType documentType, MainPageViewModel vm)
        {
            if (!vm.EmailDatas.Any())
            {
                return Array.Empty<byte>();
            }
            switch (documentType)
            {
                // doc and pdf is the same but different buttons
                case DocumentType.Doc:
                    return await GetDocAsync(
                        vm.EmailDatas.Where(x => string.Equals(x.Subject, DocumentType.Doc.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            .Select(x => new DocModel(x.Body)), false);
                case DocumentType.Pdf:
                    return await GetDocAsync(
                        vm.EmailDatas.Where(x => string.Equals(x.Subject, DocumentType.Doc.ToString(), StringComparison.CurrentCultureIgnoreCase))
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

            DocumentCore document = DocumentCore.Load(pathToDocument);

            var tables = document.GetChildElements(true, ElementType.Table).ToList().Cast<Table>().ToList();

            foreach (var (docModel, index) in payloads.Where(x 
                         => x.Section.Equals(FirstSection, StringComparison.CurrentCultureIgnoreCase)).Select((item, index) 
                         => (item, index)))
            {
                var tableRow = new TableRow(document);
                tableRow.Cells.Add(FDCell(document, (index + 1).ToString(), HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.FullName, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.Title, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.City, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.YearOfPublication, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.NumberOfPages, HorizontalAlignment.Left));
                tables[1].Rows.Add(tableRow);
            }
            foreach (var (docModel, index) in payloads.Where(x 
                         => x.Section.Equals(SecondSection, StringComparison.CurrentCultureIgnoreCase)).Select((item, index) 
                         => (item, index)))
            {
                var tableRow = new TableRow(document);
                tableRow.Cells.Add(FDCell(document, (index + 1).ToString(), HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.FullName, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.Title, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.City, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.YearOfPublication, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.NumberOfPages, HorizontalAlignment.Left));
                tables[4].Rows.Add(tableRow);
            }

            foreach (var (docModel, index) in payloads.Select((item, index) => (item, index)))
            {
                var tableRow = new TableRow(document);
                tableRow.Cells.Add(FDCell(document, (index + 1).ToString(), HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.FullName, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.Title, HorizontalAlignment.Left));

                tableRow.Cells.Add(FDCell(document, docModel.NameOfThePublication, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.TomNumber, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.NumberOfPages, HorizontalAlignment.Left));

                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tableRow.Cells.Add(new TableCell(document));
                tables[5].Rows.Add(tableRow);
            }
            var tableRowFinish = new TableRow(document);
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document, new Paragraph(document, $"Всього {payloads.Count()}"))
            {
                ColumnSpan = 5
            });
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document));
            tables[5].Rows.Add(tableRowFinish);

            using var memStream = new MemoryStream();
            if (isPdf)
            {
                document.Save(memStream, SaveOptions.PdfDefault);
            }
            else
            {
                document.Save(memStream, SaveOptions.DocxDefault);
            }
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream.ToArray();
        }


        TableCell FDCell(DocumentCore doc, string text, HorizontalAlignment alignment)
        {
            var par = new Paragraph(doc);
            par.ParagraphFormat.Alignment = alignment;
            var run = new Run(doc, text ?? "");
            run.CharacterFormat.FontName = "Times New Roman";
            run.CharacterFormat.Size = 12;
            par.Inlines.Add(run);
            return new TableCell(doc, par);
        }
    }
}