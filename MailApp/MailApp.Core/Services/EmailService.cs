using MailApp.Core.Interfaces;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MailApp.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly MainPageViewModel _mainPageViewModel;
        public EmailService(MainPageViewModel mainPageViewModel)
        {
            _mainPageViewModel = mainPageViewModel;
        }

        public async Task LoadEmails()
        {
            _mainPageViewModel.IsLoading = true;
            using var client = await GetImapClient();
            await client.Inbox.OpenAsync(FolderAccess.ReadOnly);
            var emailsIds = await client.Inbox.SearchAsync(SearchQuery.DeliveredAfter(_mainPageViewModel.From.Date).And(SearchQuery.DeliveredBefore(_mainPageViewModel.To.Date)));
            var emails = emailsIds.Select(x => client.Inbox.GetMessage(x));

            _mainPageViewModel.MimeMessages = new ObservableCollection<MimeMessage>(emails);

            _mainPageViewModel.EmailDatas = new ObservableCollection<EmailData>(_mainPageViewModel.MimeMessages.Select(x => new EmailData()
            {
                Subject = x.Subject
            }));
            _mainPageViewModel.IsLoading = false;

        }


        private async Task<ImapClient> GetImapClient()
        {
            var client = new ImapClient();
            var credentials = new NetworkCredential("test.vntu.work@gmail.com", "lyqzcabmbsfmjydh");
            var uri = new Uri("imaps://imap.gmail.com");
            await client.ConnectAsync(uri);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(credentials);
            return client;
        }
    }
}