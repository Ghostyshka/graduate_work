using System.Text.RegularExpressions;

namespace MailApp.Core.Helpers
{
    public static class RegexHelper
    {
        public static string GetString2(this Regex regex, string payload)
        {
            return regex.Match(payload).Groups[2].ToString().Trim('\r', '\n');
        }
    }
}