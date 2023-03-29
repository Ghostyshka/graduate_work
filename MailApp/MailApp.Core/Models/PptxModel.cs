using System;
using System.Text.RegularExpressions;
using MailApp.Core.Helpers;

namespace MailApp.Core.Models
{
    public class PptxModel
    {
        
        public string Section { get; set; }
        public string Author { get; set; }
        public string Topic { get; set; }
        public int Pages { get; set; }
        public string Reviewers { get; set; }

        public PptxModel(string payload)
        {
            Section = RegexConstants.SectionRegex.GetString2(payload);
            Author = RegexConstants.AuthorRegex.GetString2(payload);
            Topic = RegexConstants.TopicRegex.GetString2(payload);
            Pages = Convert.ToInt32(RegexConstants.PagesRegex.GetString2(payload));
            Reviewers = RegexConstants.ReviewersRegex.GetString2(payload);
        }
    }
}