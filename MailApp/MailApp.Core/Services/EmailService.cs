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
using MailApp.Core.Models;

namespace MailApp.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        public EmailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
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
            var credentials = new NetworkCredential(_mailSettings.Login, _mailSettings.Password);
            var uri = new Uri(_mailSettings.Url);
            await client.ConnectAsync(uri);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(credentials);
            return client;
        }
    }
}