using System.Text.RegularExpressions;

namespace MailApp.Core.Models
{
    public static class RegexConstants
    {
        public static readonly Regex NormalizeRegex = new Regex("(\\r\\n).+:");

        public static readonly Regex SectionRegex = new Regex("(Розділ: ?)(.*)(\\n)");
        public static readonly Regex AuthorRegex = new Regex("(Автор: ?)(.*)(\\n)");
        public static readonly Regex TopicRegex = new Regex("(Тема: ?)(.*)(\\n)");
        public static readonly Regex PagesRegex = new Regex("(Сторінки: ?)(.*)(\\n)");
        public static readonly Regex ReviewersRegex = new Regex("(Рецензенти: ?)((\\n|.)*$)");

        public static readonly Regex FullNameRegex = new Regex("(ПІБи: ?)(.*)(\\n)");
        public static readonly Regex TitleRegex = new Regex("(Назва роботи: ?)(.*)(\\n)");
        public static readonly Regex NameOfThePublicationRegex = new Regex("(Назва видання: ?)(.*)(\\n)");
        public static readonly Regex CityRegex = new Regex("(Місто: ?)(.*)(\\n)");
        public static readonly Regex YearOfPublicationRegex = new Regex("(Рік видання: ?)(.*)(\\n)");
        public static readonly Regex NumberOfPagesRegex = new Regex("(Сторінки: ?)(.*)(\\n)");
        public static readonly Regex TomNumberRegex = new Regex("(Том, номер: ?)(.*)(\\n)");

        public static readonly Regex CategoryARegex = new Regex("(Статті у фахових виданнях України категорії А: ?)(.*)(\\n)");
        public static readonly Regex CategoryBRegex = new Regex("(Статті у фахових виданнях України категорії Б: ?)(.*)(\\n)");
        public static readonly Regex InternationalPublicationsRegex = new Regex("(Статті у закордонних виданнях: ?)(.*)(\\n)");

        public static readonly Regex ScopusPublicationsRegex = new Regex("(Статті у наукових журналах, \\(МНМБД\\) і Scopus: ?)(.*)(\\n)");
        public static readonly Regex WoSPublicationsRegex = new Regex("(Статті у наукових журналах, \\(WoS\\): ?)(.*)(\\n)");
        
        public static readonly Regex CopernicusPublicationsRegex = new Regex("(Статті у журналах, збірниках, Copernicus: ?)(.*)(\\n)");
        public static readonly Regex OtherPublicationsRegex = new Regex("(Інші статті: ?)(.*)(\\n)");
        public static readonly Regex CoreCollectionPublicationsRegex = new Regex("(Статті у наукових журналах, з Web of Core Collection, з \\(JIF\\) або \\(JCI\\): ?)(.*)(\\n)");
        public static readonly Regex QCategoryPublicationsRegex = new Regex("(Статті у наукових журналах, Q1 - Q4 \\(зазначити категорію\\): ?)(.*)(\\n)");
        public static readonly Regex ConferenceThesisRegex = new Regex("(Тези доповідей: ?)(.*)(\\n)");

    }
}