using MailApp.Core.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MailApp.Core.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {

        }

        public async Task<IEnumerable<MimeMessage>> LoadEmails(DateTime from, DateTime to)
        {
            using var client = await GetImapClient();
            await client.Inbox.OpenAsync(FolderAccess.ReadOnly);
            var emailsIds = await client.Inbox.SearchAsync(SearchQuery.DeliveredAfter(from).And(SearchQuery.DeliveredBefore(to)));
            var emails = emailsIds.Select(x => client.Inbox.GetMessage(x)).ToList();
            return emails;
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