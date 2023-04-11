using MailApp.Core.Helpers;

namespace MailApp.Core.Models
{
    public class DocModel
    {
        public string Section { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string NameOfThePublication { get; set; }
        public string City { get; set; }
        public string YearOfPublication { get; set; }
        public string NumberOfPages { get; set; }
        public string TomNumber { get; set; }
        public DocModel(string payload)
        {
            Section = RegexConstants.SectionRegex.GetString2(payload);
            FullName = RegexConstants.FullNameRegex.GetString2(payload);
            Title = RegexConstants.TitleRegex.GetString2(payload);
            NameOfThePublication = RegexConstants.NameOfThePublicationRegex.GetString2(payload);
            City = RegexConstants.CityRegex.GetString2(payload);
            YearOfPublication = RegexConstants.YearOfPublicationRegex.GetString2(payload);
            NumberOfPages = RegexConstants.NumberOfPagesRegex.GetString2(payload);
            TomNumber = RegexConstants.TomNumberRegex.GetString2(payload);
        }
    }
}