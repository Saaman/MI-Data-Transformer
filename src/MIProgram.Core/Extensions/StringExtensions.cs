using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;
using System.Text;

namespace MIProgram.Core.Extensions
{
    public static class BaseTypesExtensions
    {
        public static string ToCamelCase(this string input)
        {
            var stringParts = new List<string>(input.Split(' '));
            string result = string.Empty;
            result = stringParts.Aggregate(result, (current, x) => current + (x[0].ToString()).ToUpperInvariant() + x.ToLowerInvariant().Substring(1) + " ");
            return result.TrimEnd();
        }

        public static string ToRailsSym(this string input)
        {
            return ":" + input.ToLowerInvariant().RemoveDiacritics().Split(' ').Aggregate((res, next) => res + "_" + next);
        }

        public static string ToSQLValue(this string input)
        {
            return input.Replace("'", "''");
        }

        public static string GetSafeMySQL(this string myString)
        {
            return myString.Replace("'", "''");
        }

        public static string GetSafeRails(this string myString)
        {
            return myString.Replace("’", "''").Replace("'", "''").Replace("…", "...");
        }

        public static int ToUnixTimeStamp(this DateTime dateTime)
        {
            var diff = dateTime - new DateTime(1970, 1, 1).ToLocalTime();
            return (int)diff.TotalSeconds;
        }

        public static string RemoveDiacritics(this string stIn)
        {
            var stFormD = stIn.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var t in stFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}