using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2DRakun.Code
{
    public class IntHelper
    {
        /// <summary>
        /// Attempts to parse a string into an integer.
        /// If the input is <c>null</c>, empty, or parsing fails,
        /// the specified fallback value is returned.
        /// </summary>
        /// <param name="input">
        /// The string value to parse into an <see cref="int"/>.
        /// </param>
        /// <param name="res">
        /// The value to return if parsing fails.
        /// Defaults to <c>0</c>.
        /// </param>
        /// <returns>
        /// The parsed integer if parsing succeeds;
        /// otherwise, returns the value specified by <paramref name="res"/>.
        /// </returns>
        public static int TryParseInt(string input, int res = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return res;
            }
            if (int.TryParse(input, out int i))
            {
                return i;
            }
            else
            {
                return res;
            }
        }
    }
}