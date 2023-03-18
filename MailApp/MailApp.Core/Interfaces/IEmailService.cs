using MimeKit;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace MailApp.Core.Interfaces
{
    public interface IEmailService
    {
        Task<IEnumerable<MimeMessage>> LoadEmails(DateTime from, DateTime to);
    }
}