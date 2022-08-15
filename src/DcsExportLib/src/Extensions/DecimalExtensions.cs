using System.Globalization;

namespace DcsExportLib.Extensions
{
    internal static class DecimalExtensions
    {
        /// <summary>
        /// Rounds the number to given number of decimal places and removes the trailing zeroes
        /// </summary>
        /// <param name="value">Value to round and trim</param>
        /// <param name="decimalPlacesCount">Number of decimal places</param>
        /// <returns>Rounded and trimmed decimal value</returns>
        /// <exception cref="ArgumentException">Exception of wrong number of decimal places. Must be > 1</exception>
        public static decimal Trail(this decimal value, int decimalPlacesCount)
        {
            if (decimalPlacesCount <= 0)
                throw new ArgumentException("Unexpected number of decimal places for trailing.");

            string formatString = "0.";

            for (int i = 0; i < decimalPlacesCount; i++)
                formatString += "#";

            string strValue = value.ToString(formatString, CultureInfo.InvariantCulture);
            return Decimal.Parse(strValue);
        }
    }
}
