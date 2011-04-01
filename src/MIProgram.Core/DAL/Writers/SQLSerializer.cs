using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIProgram.Core.Extensions;
using MIProgram.Core.Model;
using MIProgram.Core.Writers;
using MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder;

namespace MIProgram.Core.DAL.Writers
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
                sb.AppendFormat("INSERT INTO  `midatabase`.`mi_artist_country` (`country_name` , `country_code`, `country_term_alias`) VALUES ('{0}',  '{1}', '{2}');", country.Key.ToCamelCase().ToSQLValue(), country.Value.ToSQLValue(), "Pays/" + country.Key.ToCamelCase().ToSQLValue());
                sb.AppendLine();
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeReviewers(IList<Reviewer> reviewers, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader(Reviewer.SQLTableName));

            foreach (var reviewer in reviewers)
            {
                sb.AppendLine(reviewer.ToSQLInsert());
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeArtists(IList<Artist> newArtists, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader(Artist.SQLTableName));

            foreach (var artist in newArtists)
            {
                sb.AppendLine(artist.ToSQLInsert());
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeAlbumTypes(List<string> albumTypes, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader("mi_album_type"));

            foreach (var albumType in albumTypes)
            {
                sb.AppendFormat("INSERT INTO  `midatabase`.`mi_album_type` (`album_type`) VALUES ('{0}');", albumType.ToCamelCase());
                sb.AppendLine();
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeAlbumStyles(IList<StylesTreeItem> albumStyles, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader(StylesTree.SQLTableName));

            foreach (var albumStyle in albumStyles)
            {
                sb.AppendLine(albumStyle.ToSQLInsert());
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }
    }
}