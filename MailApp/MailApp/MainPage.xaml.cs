﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Autofac;
using MailApp.Core.Interfaces;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;
using MimeKit;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Text.RegularExpressions;
using System.Security.Principal;
using Org.BouncyCastle.Asn1.X509;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MailApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel _vm;
        public MainPage()
        {
            this.InitializeComponent();
            _vm = App.Container.Resolve<MainPageViewModel>();
            DataContext = _vm;
        }

        private async void DOWNLOAD_Click(object sender, RoutedEventArgs e)
        {
            await LoadEmails();
        }

        private async Task LoadEmails()
        {
            var vm = App.Container.Resolve<MainPageViewModel>();
            vm.IsLoading = true;
            var emailService = App.Container.Resolve<IEmailService>();
            var emails = (await emailService.LoadEmails(vm.From.Date, vm.To.Date))
                .Where(x => Enum.GetNames(typeof(DocumentType)).Contains(x.Subject, StringComparer.InvariantCultureIgnoreCase));

            vm.MimeMessages = new ObservableCollection<MimeMessage>(emails);

            vm.EmailDatas = new ObservableCollection<EmailData>(vm.MimeMessages.Select(x => new EmailData()
            {
                Subject = x.Subject,
                DateTime = x.Date.DateTime,
                Body = x.TextBody,
                Sender = x.From?.Mailboxes.First().Name + '\n' + x.From?.Mailboxes.First().Address
            }));

            vm.IsLoading = false;
        }

        private async void DOC_Click(object sender, RoutedEventArgs e)
        {
            await SaveDocumentTask(DocumentType.Docx);
        }

        private async void PPTX_Click(object sender, RoutedEventArgs e)
        {
            await SaveDocumentTask(DocumentType.Pptx);
        }

        private async void PDF_Click(object sender, RoutedEventArgs e)
        {
            await SaveDocumentTask(DocumentType.Pdf);
        }

        private async Task SaveDocumentTask(DocumentType documentType, WriteMode mode = WriteMode.New)
        {
            var vm = App.Container.Resolve<MainPageViewModel>();
            var documetService = App.Container.Resolve<IDocumentService>();

            StorageFile file = null;

            switch (mode)
            {
                case WriteMode.New:
                    {
                        var filePicker = new FileSavePicker();
                        filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                        filePicker.FileTypeChoices.Add(documentType.ToString(), new List<string>() { $".{documentType.ToString().ToLower()}" });
                        filePicker.SuggestedFileName = $"{documentType} {DateTime.Now}";

                        file = await filePicker.PickSaveFileAsync();

                        if (file == null || !file.IsAvailable)
                        {
                            return;
                        }
                        break;
                    }
                case WriteMode.Old:
                    {
                        var filePicker = new FileOpenPicker();
                        filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                        filePicker.FileTypeFilter.Add($".{documentType.ToString().ToLower()}");

                        file = await filePicker.PickSingleFileAsync();


                        if (file == null || !file.IsAvailable)
                        {
                            return;
                        }
                        break;
                    }
                default:
                    throw new Exception("invalid write mode");
            }

            

            vm.IsLoading = true;
            
            var doc = await documetService.GetDocumentAsync(documentType, vm, mode, file);
            CachedFileManager.DeferUpdates(file);
            await FileIO.WriteBytesAsync(file, doc);

            Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            vm.IsLoading = false;
        }

        private async void AddDoc_OnClick(object sender, RoutedEventArgs e)
        {
            await SaveDocumentTask(DocumentType.Docx, WriteMode.Old);
        }
    }
}
