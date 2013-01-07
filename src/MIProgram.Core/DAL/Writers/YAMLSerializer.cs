using System;
using System.Collections.Generic;
using System.Text;
using MIProgram.Core.Logging;
using MIProgram.Core.Model;

namespace MIProgram.Core.DAL.Writers
{
    public class YAMLSerializer
    {
        private readonly IWriter _fileWriter;
        private readonly string _outputDir;

        public YAMLSerializer(IWriter writer, string outputDir)
        {
            _fileWriter = writer;
            _outputDir = outputDir;
        }

        public void CleanPreviousFile(string fileName)
        {
            _fileWriter.CleanFile(fileName, _outputDir);
        }

        //public void SerializeCountries(IDictionary<string, string> countriesLabelsAndCodes, string fileName)
        //{
        //    var sb = new StringBuilder();

        //    sb.AppendLine(GetHeader("mi_artist_country"));

        //    foreach (var country in countriesLabelsAndCodes)
        //    {
        //        sb.AppendFormat("INSERT INTO `mi_artist_country` (`country_name` , `country_code`, `country_term_alias`) VALUES ('{0}',  '{1}', '{2}');", country.Key.ToCamelCase().ToSQLValue(), country.Value.ToSQLValue(), "Pays/" + country.Key.ToCamelCase().ToSQLValue());
        //        sb.AppendLine();
        //    }

        //    _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        //}

        public void SerializeReviewers(IList<Reviewer> reviewers, string fileName)
        {
            var sb = new StringBuilder();

            foreach (var reviewer in reviewers)
            {
                try
                {
                    sb.AppendLine("---");
                    sb.AppendLine(reviewer.ToYAMLInsert());
                }
                catch (Exception e)
                {
                    Logging.Logging.Instance.LogError(string.Format("Reviewer {0}-{1} : {2}", reviewer.Id, reviewer.Name, e.Message), ErrorLevel.Warning);
                }
            }

            _fileWriter.WriteYAML(sb.ToString(), fileName, _outputDir);
        }

        public void SerializeArtists(IList<Artist> newArtists, string fileName)
        {
            var sb = new StringBuilder();

            foreach (var artist in newArtists)
            {
                try
                {
                    sb.AppendLine("---");
                    sb.AppendLine(artist.ToYAMLInsert());
                }
                catch(Exception e)
                {
                    Logging.Logging.Instance.LogError(string.Format("Artist {0}-{1} : {2}", artist.Id, artist.Name, e.Message), ErrorLevel.Warning);
                }
            }

            _fileWriter.WriteYAML(sb.ToString(), fileName, _outputDir);
        }

        //public void SerializeAlbumTypes(List<string> albumTypes, string fileName)
        //{
        //    var sb = new StringBuilder();

        //    sb.AppendLine(GetHeader("mi_album_type"));

        //    foreach (var albumType in albumTypes)
        //    {
        //        sb.AppendFormat("INSERT INTO `mi_album_type` (`album_type`) VALUES ('{0}');", albumType.ToCamelCase());
        //        sb.AppendLine();
        //    }

        //    _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        //}

        public void SerializeLabelVendors(IList<LabelVendor> labelVendors, string fileName)
        {
            var sb = new StringBuilder();

            foreach (var labelVendor in labelVendors)
            {
                sb.AppendLine(labelVendor.ToRailsInsert());
            }

            _fileWriter.WriteRB(sb.ToString(), fileName, _outputDir);
        }

        //public void SerializeAlbumStyles(IList<StylesTreeItem> albumStyles, string fileName)
        //{
        //    var sb = new StringBuilder();

        //    sb.AppendLine(GetHeader(StylesTree.SQLTableName));

        //    foreach (var albumStyle in albumStyles)
        //    {
        //        sb.AppendLine(albumStyle.ToSQLInsert());
        //    }

        //    _fileWriter.WriteSQL(sb.ToString(), fileName, _outputDir);
        //}

        public void SerializeAlbums(List<Album> albums, string fileName)
        {
            var sb = new StringBuilder();

            foreach (var album in albums)
            {
                try
                {
                    sb.AppendLine(album.ToRailsInsert());
                }
                catch (Exception e)
                {
                    Logging.Logging.Instance.LogError(string.Format("Album {0}-{1} : {2}",album.Id, album.Title, e.Message), ErrorLevel.Warning);
                }
            }

            _fileWriter.WriteRB(sb.ToString(), fileName, _outputDir);
        }
    }
}