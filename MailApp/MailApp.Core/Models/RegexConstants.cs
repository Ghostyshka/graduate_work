using System.Text.RegularExpressions;

namespace MailApp.Core.Models
{
    public static class RegexConstants
    {
        public static readonly Regex SectionRegex = new Regex("(Розділ:) (.*)(\\n)");
        public static readonly Regex AuthorRegex = new Regex("(Автор:) (.*)(\\n)");
        public static readonly Regex TopicRegex = new Regex("(Тема:) (.*)(\\n)");
        public static readonly Regex PagesRegex = new Regex("(Сторінки:) (.*)(\\n)");
        public static readonly Regex ReviewersRegex = new Regex("(Рецензенти:) (.*)(\\n)");

        public static readonly Regex FullNameRegex = new Regex("(ПІБи:) (.*)(\\n)");
        public static readonly Regex TitleRegex = new Regex("(Назва роботи:) (.*)(\\n)");
        public static readonly Regex NameOfThePublicationRegex = new Regex("(Назва видання:) (.*)(\\n)");
        public static readonly Regex CityRegex = new Regex("(Місто:) (.*)(\\n)");
        public static readonly Regex YearOfPublicationRegex = new Regex("(Рік видання:) (.*)(\\n)");
        public static readonly Regex NumberOfPagesRegex = new Regex("(Сторінки:) (.*)(\\n)");
        public static readonly Regex TomNumberRegex = new Regex("(Том, номер:) (.*)(\\n)");
    }
}