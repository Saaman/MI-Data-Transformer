using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIProgram.Core.Writers;
using MIProgram.Model;
using MIProgram.Model.Extensions;

namespace MIProgram.Core
{
    public class SQLSerializer
    {
        private readonly IWriter _fileWriter;
        private readonly string _outputDir;
        
        public SQLSerializer(IWriter writer, string outputDir)
        {
            _fileWriter = writer;
            _outputDir = outputDir;
        }

        public static string GetHeader(string tableName)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM `{0}`;", tableName);
            sb.AppendLine();
            sb.AppendFormat("ALTER TABLE `{0}` AUTO_INCREMENT=0;", tableName);
            sb.AppendLine();
            return sb.ToString();
        }

        public void SerializeCountries(IDictionary<string, string> countriesLabelsAndCodes, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader("mi_artist_country"));

            foreach (var country in countriesLabelsAndCodes)
            {
                sb.AppendFormat("INSERT INTO  `midatabase`.`MI_artist_country` (`country_name` , `country_code`, `country_term_alias`) VALUES ('{0}',  '{1}', '{2}');", country.Key.ToCamelCase().ToSQLValue(), country.Value.ToSQLValue(), "Pays/" + country.Key.ToCamelCase().ToSQLValue());
                sb.AppendLine();
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeReviewers(IList<Reviewer> reviewers, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader("mi_artist_creator"));

            foreach (var reviewer in reviewers)
            {
                sb.AppendFormat("INSERT INTO  `midatabase`.`mi_artist_creator` (`reviewer_id`, `name`, `creation_date`, `mail`, `password`) VALUES ('{0}',  '{1}',  '{2}',  '{3}',  '{4}');", reviewer.Id, reviewer.Name, reviewer.CreationDate.ToUnixTimeStamp(), reviewer.MailAddress, reviewer.Password);
                sb.AppendLine();
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeArtists(IList<Artist> newArtists, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader("mi_artist"));

            foreach (var artist in newArtists)
            {
                sb.AppendFormat(
                    "INSERT INTO  `midatabase`.`mi_artist` (`artist_id`, `name`, `creation_date`, `official_site`, `reviewer_id`, `countries`, `similar_artists_nodes`, `additionnal_similar_artists_names`, `sorting_weight`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');"
                    , artist.Id, artist.Name.ToCamelCase().GetSafeMySQL(), artist.CreationDate.ToUnixTimeStamp()
                    , artist.OfficialUrl, artist.Reviewer.Id
                    , artist.Countries.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.ToCamelCase()).Trim('|')
                    , artist.SimilarArtists.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.Id).Trim('|')
                    , artist.ArtistSimilarArtistsNames.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.ToCamelCase()).Trim('|')
                    , artist.SortWeight);
                sb.AppendLine();
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }
    }

    #region Addtionnal extensions classes

    public static class DateTimeExtensions
    {
        public static int ToUnixTimeStamp(this DateTime dateTime)
        {
            var diff = dateTime - new DateTime(1970, 1, 1).ToLocalTime();
            return (int) diff.TotalSeconds;
        }
    }

    public static class StringExtensions
    {
        public static string GetSafeMySQL(this string myString)
        {
            return myString.Replace("'", "''");
        }
    }

    #endregion
}
