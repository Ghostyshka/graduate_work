using System;

namespace MailApp.Core.Models
{
    public class EmailData
    {
        public string Subject { get; set; }
        public string Sender { get; set; }
        public DateTime DateTime { get; set; }
        public string Body { get; set; }
    }
}