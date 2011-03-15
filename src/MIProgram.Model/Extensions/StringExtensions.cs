using System.Collections.Generic;
using System.Linq;

namespace MIProgram.Model.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            var stringParts = new List<string>(input.Split(' '));
            string result = string.Empty;
            result = stringParts.Aggregate(result, (current, x) => current + (x[0].ToString()).ToUpperInvariant() + x.ToLowerInvariant().Substring(1) + " ");
            return result.TrimEnd();
        }

        public static string ToSQLValue(this string input)
        {
            return input.Replace("'", "''");
        }
    }
}