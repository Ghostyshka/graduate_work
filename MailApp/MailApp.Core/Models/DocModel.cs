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
        public string CategoryA { get; set; }
        public string CategoryB { get; set; }
        public string InternationalPublications { get; set; }
        public string ScopusPublications { get; set; }
        public string WoSPublications { get; set; }
        public string CopernicusPublications { get; set; }
        public string OtherPublications { get; set; }
        public string CoreCollectionPublications { get; set; }
        public string QCategoryPublications { get; set; }
        public string ConferenceThesis { get; set; }

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
            CategoryA = RegexConstants.CategoryARegex.GetString2(payload);
            CategoryB = RegexConstants.CategoryBRegex.GetString2(payload);
            InternationalPublications = RegexConstants.InternationalPublicationsRegex.GetString2(payload);
            ScopusPublications = RegexConstants.ScopusPublicationsRegex.GetString2(payload);
            WoSPublications = RegexConstants.WoSPublicationsRegex.GetString2(payload);
            CopernicusPublications = RegexConstants.CopernicusPublicationsRegex.GetString2(payload);
            OtherPublications = RegexConstants.OtherPublicationsRegex.GetString2(payload);
            CoreCollectionPublications = RegexConstants.CoreCollectionPublicationsRegex.GetString2(payload);
            QCategoryPublications = RegexConstants.QCategoryPublicationsRegex.GetString2(payload);
            ConferenceThesis = RegexConstants.ConferenceThesisRegex.GetString2(payload);
        }
    }
}