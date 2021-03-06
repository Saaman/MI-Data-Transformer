﻿using System;
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
        private readonly string _fileName;

        public YAMLSerializer(IWriter writer, string outputDir, string fileName)
        {
            _fileWriter = writer;
            _outputDir = outputDir;
            _fileName = fileName;
        }

        public void CleanPreviousFile()
        {
            _fileWriter.CleanFile(_fileName, _outputDir);
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

        public void SerializeReviewers(IList<Reviewer> reviewers)
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

            _fileWriter.WriteYAML(sb.ToString(), _fileName, _outputDir);
        }

        public void SerializeArtists(IList<Artist> newArtists)
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

            _fileWriter.WriteYAML(sb.ToString(), _fileName, _outputDir);
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

        public void SerializeLabelVendors(IList<LabelVendor> labelVendors)
        {
            var sb = new StringBuilder();

            foreach (var labelVendor in labelVendors)
            {
                try
                {
                    sb.AppendLine("---");
                    sb.AppendLine(labelVendor.ToYAMLInsert());
                }
                catch (Exception e)
                {
                    Logging.Logging.Instance.LogError(string.Format("LabelVendor {0}-{1} : {2}", labelVendor.Id, labelVendor.Label, e.Message), ErrorLevel.Warning);
                }
            }

            _fileWriter.WriteYAML(sb.ToString(), _fileName, _outputDir);
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

        public void SerializeAlbums(List<Album> albums)
        {
            var sb = new StringBuilder();

            foreach (var album in albums)
            {
                try
                {
                    sb.AppendLine("---");
                    sb.AppendLine(album.ToYAMLInsert());
                }
                catch (Exception e)
                {
                    Logging.Logging.Instance.LogError(string.Format("Album {0}-{1} : {2}", album.Id, album.Title, e.Message), ErrorLevel.Warning);
                }
            }

            _fileWriter.WriteYAML(sb.ToString(), _fileName, _outputDir);
        }

        public void CloseFile()
        {
            _fileWriter.WriteYAML("---", _fileName, _outputDir);
        }

        public void SerializeReviews(List<Review> reviews)
        {
            var sb = new StringBuilder();

            foreach (var review in reviews)
            {
                try
                {
                    sb.AppendLine("---");
                    sb.AppendLine(review.ToYAMLInsert());
                }
                catch (Exception e)
                {
                    Logging.Logging.Instance.LogError(string.Format("Review on album {0}-{1} : {2}", review.Product.Id, review.Product.Title, e.Message), ErrorLevel.Warning);
                }
            }

            _fileWriter.WriteYAML(sb.ToString(), _fileName, _outputDir);
        }
    }
}