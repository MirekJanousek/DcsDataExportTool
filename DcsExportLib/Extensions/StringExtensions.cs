using System.Text.RegularExpressions;

namespace DcsClickableExportLib.Extensions
{
    internal static class StringExtensions
    {
        public static int IndexOfLineEnd(this string str, int startIndex)
        {
            if(string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(nameof(str));

            int ix = str.IndexOf("\r", startIndex, StringComparison.Ordinal);

            if (ix == -1)
            {
                ix = str.IndexOf("\n", startIndex, StringComparison.Ordinal);
            }
            
            return ix;
        }

        public static string FindQuotedToLineEnd(this string str)
        {
            // find the starting quotation char
            int ix1 = str.IndexOf("\"", StringComparison.Ordinal);
            int ix2 = str.IndexOf("\'", StringComparison.Ordinal);

            if (ix1 == -1 && ix2 == -1)
                return String.Empty;

            ix1 = ix1 == -1 ? int.MaxValue : ix1;
            ix2 = ix2 == -1 ? int.MaxValue : ix2;

            if (ix1 < ix2)
            {
                return FindQuotedToLineEndInternal(str, '"');
            }
            else
            {
                return FindQuotedToLineEndInternal(str, '\'');
            }
        }

        private static string FindQuotedToLineEndInternal(string str, char quotationMark)
        {
            // Match pattern: search for text between ""
            string regexPattern = "\\" + quotationMark + "(.*?)\\" + quotationMark;
            Regex r = new Regex(regexPattern);
            Match m = r.Match(str);

            if (!m.Success)
            {
                return String.Empty;
            }

            // TODO MJ: make sure its valid string
            return m.Value.Trim(quotationMark);
        }
    }
}
