using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using GemBox.Presentation;
using MailApp.Core.Interfaces;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;
using SautinSoft.Document;
using SautinSoft.Document.Tables;
using HorizontalAlignment = SautinSoft.Document.HorizontalAlignment;
using LoadOptions = SautinSoft.Document.LoadOptions;
using SaveOptions = SautinSoft.Document.SaveOptions;

namespace MailApp.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private const string FirstSection = "Монографії";
        private const string SecondSection = "Навчальні посібники";
        public async Task<byte[]> GetDocumentAsync(DocumentType documentType, MainPageViewModel vm, WriteMode mode, StorageFile file)
        {
            if (!vm.EmailDatas.Any())
            {
                return Array.Empty<byte>();
            }
            switch (documentType)
            {
                // doc and pdf is the same but different buttons
                case DocumentType.Docx:
                    return await GetDocAsync(
                        vm.EmailDatas
                            .Where(x => x.IsSelected && string.Equals(x.Subject, DocumentType.Docx.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            .Select(x => new DocModel(x.Body)), false, mode, file);
                case DocumentType.Pdf:
                    return await GetDocAsync(
                        vm.EmailDatas
                            .Where(x => x.IsSelected && string.Equals(x.Subject, DocumentType.Docx.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            .Select(x => new PdfModel(x.Body)), true, mode, file);
                case DocumentType.Pptx:
                    return await GetPptxAsync(vm.EmailDatas
                            .Where(x => x.IsSelected && string.Equals(x.Subject, DocumentType.Pptx.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            .Select(x => new PptxModel(x.Body)));
                default:
                    return Array.Empty<byte>();
            }
        }

        private async Task<byte[]> GetPptxAsync(IEnumerable<PptxModel> payloads)
        {

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var pathToDocument = AppDomain.CurrentDomain.BaseDirectory + "Assets\\PPTX.pptx";
            var presentation = PresentationDocument.Load(pathToDocument);
            var grouped = payloads.GroupBy(x => x.Section);

            var sourceTemplateSlide = presentation.Slides[2];
            var sourceShapeEndText = presentation.Slides[3].Content.Drawings[0];
            var sourceShapeSection = presentation.Slides[3].Content.Drawings[1];


            foreach (var group in grouped)
            {
                var data = group.ElementAt(0);
                var newSlide = presentation.Slides.InsertClone(presentation.Slides.Count - 3, sourceTemplateSlide);

                var sourceShape = newSlide.Content.Drawings.ElementAt(1);
                var targetShape = newSlide.Content.Drawings.AddClone(sourceShape);

                targetShape.TextContent.Replace("##автори##", data.Author);
                targetShape.TextContent.Replace("##тема##", data.Topic);
                targetShape.TextContent.Replace("##сторінки##", data.Pages.ToString());
                targetShape.TextContent.Replace("##рецензенти##", data.Reviewers);

                targetShape = newSlide.Content.Drawings.AddClone(sourceShapeSection);
                targetShape.TextContent.Replace("##розділ##", data.Section);

                for (int i = 1; i <= group.Count() - 1; i++)
                {
                    data = group.ElementAt(i);

                    newSlide = presentation.Slides.InsertClone(presentation.Slides.Count - 3, sourceTemplateSlide);

                    sourceShape = newSlide.Content.Drawings.ElementAt(1);
                    targetShape = newSlide.Content.Drawings.AddClone(sourceShape);

                    targetShape.TextContent.Replace("##автори##", data.Author);
                    targetShape.TextContent.Replace("##тема##", data.Topic);
                    targetShape.TextContent.Replace("##сторінки##", data.Pages.ToString());
                    targetShape.TextContent.Replace("##рецензенти##", data.Reviewers);
                }
                data = group.Last();
                targetShape = newSlide.Content.Drawings.AddClone(sourceShapeEndText);
                targetShape.TextContent.Replace("##розділ##", data.Section);
            }
            presentation.Slides.RemoveAt(presentation.Slides.Count - 1);
            presentation.Slides.RemoveAt(presentation.Slides.Count - 1);

            MemoryStream presentationStream = new MemoryStream();
            presentation.Save(presentationStream, GemBox.Presentation.SaveOptions.Pptx);

            byte[] presentationBytes = presentationStream.ToArray();
            presentationStream.Close();
            return presentationBytes;
        }







        private async Task<byte[]> GetDocAsync(IEnumerable<DocModel> payloads, bool isPdf, WriteMode mode, StorageFile file)
        {
            
            DocumentCore document = null;
                
            switch (mode)
            {
                case WriteMode.New:
                {
                    var pathToDocument = AppDomain.CurrentDomain.BaseDirectory + "Assets\\DOC.docx";
                    document = DocumentCore.Load(pathToDocument);
                    break;
                }
                case WriteMode.Old:
                {
                    document = DocumentCore.Load((await FileIO.ReadBufferAsync(file)).AsStream(), LoadOptions.DocxDefault);
                    break;
                }
                default:
                {
                    throw new Exception("Invalid mode");
                }
            }

            var tables = document.GetChildElements(true, ElementType.Table).ToList().Cast<Table>().ToList();

            var existingRowCount = tables[1].Rows.Count - 1;

            foreach (var (docModel, index) in payloads.Where(x 
                         => x.Section.Equals(FirstSection, StringComparison.CurrentCultureIgnoreCase)).Select((item, index) 
                         => (item, index)))
            {
                var tableRow = new TableRow(document);
                tableRow.Cells.Add(FDCell(document, (index + 1 + existingRowCount).ToString(), HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.FullName, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.Title, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.City, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.YearOfPublication, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.NumberOfPages, HorizontalAlignment.Left));
                tables[1].Rows.Add(tableRow);
            }
            existingRowCount = tables[4].Rows.Count - 1;
            foreach (var (docModel, index) in payloads.Where(x 
                         => x.Section.Equals(SecondSection, StringComparison.CurrentCultureIgnoreCase)).Select((item, index) 
                         => (item, index)))
            {
                var tableRow = new TableRow(document);
                tableRow.Cells.Add(FDCell(document, (index + 1 + existingRowCount).ToString(), HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.FullName, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.Title, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.City, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.YearOfPublication, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.NumberOfPages, HorizontalAlignment.Left));
                tables[4].Rows.Add(tableRow);
            }

            if (mode == WriteMode.Old)
            {
                existingRowCount = tables[5].Rows.Count - 3;
                tables[5].Rows.RemoveAt(tables[5].Rows.Count - 1);
            }
            else
            {
                existingRowCount = 0;
            }
            
            foreach (var (docModel, index) in payloads.Select((item, index) => (item, index)))
            {
                var tableRow = new TableRow(document);
                tableRow.Cells.Add(FDCell(document, (index + 1 + existingRowCount).ToString(), HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.FullName, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.Title, HorizontalAlignment.Left));

                tableRow.Cells.Add(FDCell(document, docModel.NameOfThePublication, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.TomNumber, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.NumberOfPages, HorizontalAlignment.Left));

                tableRow.Cells.Add(FDCell(document, docModel.CategoryA, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.CategoryB, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.InternationalPublications, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.ScopusPublications, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.WoSPublications, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.CopernicusPublications, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.OtherPublications, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.CoreCollectionPublications, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.QCategoryPublications, HorizontalAlignment.Left));
                tableRow.Cells.Add(FDCell(document, docModel.ConferenceThesis, HorizontalAlignment.Left));
                
                tables[5].Rows.Add(tableRow);
            }

            
            var tableRowFinish = new TableRow(document);
            tableRowFinish.Cells.Add(new TableCell(document));
            tableRowFinish.Cells.Add(new TableCell(document, new Paragraph(document, $"Всього {payloads.Count() + existingRowCount}"))
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