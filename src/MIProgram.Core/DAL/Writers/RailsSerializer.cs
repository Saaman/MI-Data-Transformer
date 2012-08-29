using System.Collections.Generic;
using System.Text;
using MIProgram.Core.Extensions;
using MIProgram.Core.Model;
using MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder;

namespace MIProgram.Core.DAL.Writers
{
    public class RailsSerializer
    {
        private readonly IWriter _fileWriter;
        private readonly string _outputDir;

        public RailsSerializer(IWriter writer, string outputDir)
        {
            _fileWriter = writer;
            _outputDir = outputDir;
        }

        private static string GetHeader(string modelName, bool overrideTimeStamps = true)
        {
            var sb = new StringBuilder();
            if (overrideTimeStamps)
            {
                sb.AppendLine("ActiveRecord::Base.record_timestamps = false");
                sb.AppendLine();
            }
            sb.AppendFormat("{0}s = []", modelName);
            sb.AppendLine();
            return sb.ToString();
        }

        private static string GetFooter(string modelName, bool overrideTimeStamps = true)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("bulk_save({0}s)", modelName);
            sb.AppendLine();
            if (overrideTimeStamps)
                sb.AppendLine("ActiveRecord::Base.record_timestamps = true");
            return sb.ToString();
        }

        public void SerializeCountries(IDictionary<string, string> countriesLabelsAndCodes, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader("mi_artist_country"));

            foreach (var country in countriesLabelsAndCodes)
            {
                sb.AppendFormat("INSERT INTO `mi_artist_country` (`country_name` , `country_code`, `country_term_alias`) VALUES ('{0}',  '{1}', '{2}');", country.Key.ToCamelCase().ToSQLValue(), country.Value.ToSQLValue(), "Pays/" + country.Key.ToCamelCase().ToSQLValue());
                sb.AppendLine();
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeReviewers(IList<Reviewer> reviewers, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader(Reviewer.RailsModelName));

            foreach (var reviewer in reviewers)
            {
                sb.AppendLine(reviewer.ToRailsInsert());
            }

            sb.Append(GetFooter(Reviewer.RailsModelName));

            _fileWriter.WriteRB(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeArtists(IList<Artist> newArtists, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader(Artist.RailsModelName));

            foreach (var artist in newArtists)
            {
                sb.AppendLine(artist.ToRailsInsert());
            }

            sb.Append(GetFooter(Artist.RailsModelName));

            _fileWriter.WriteRB(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeAlbumTypes(List<string> albumTypes, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader("mi_album_type"));

            foreach (var albumType in albumTypes)
            {
                sb.AppendFormat("INSERT INTO `mi_album_type` (`album_type`) VALUES ('{0}');", albumType.ToCamelCase());
                sb.AppendLine();
            }

            _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeLabelVendors(IList<LabelVendor> labelVendors, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader(LabelVendor.RailsModelName, false));

            foreach (var labelVendor in labelVendors)
            {
                sb.AppendLine(labelVendor.ToRailsInsert());
            }

            sb.Append(GetFooter(LabelVendor.RailsModelName, false));

            _fileWriter.WriteRB(sb.ToString(), fileName, _outputDir);
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

        public void SerializeAlbums(List<Album> albums, string fileName)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader(Album.RailsModelName));

            foreach (var album in albums)
            {
                sb.AppendLine(album.ToRailsInsert());
            }

            sb.Append(GetFooter(Album.RailsModelName));

            _fileWriter.WriteRB(sb.ToString(), fileName, _outputDir);
        }
    }
}