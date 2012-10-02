﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIProgram.Core.Extensions;

namespace MIProgram.Core.Model
{
    public class Album : Product
    {
        public const string SQLTableName = "mi_album";
        public const string RailsModelName = "album";

        //Existing
        public string Title { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public int Score { get; private set; }
        public string Label { get; private set; }
        public string AlbumType { get; private set; }
        public string CoverPath { get; private set; }

        public string DeezerAlbum { get; private set;}
        public string DeezerArtist { get; private set; }
        public string ReviewText { get; private set; }

        public LabelVendor LabelVendor { get; set; }
        public IList<string> RawSimilarAlbums { get; set; }
        public IList<Album> SimilarAlbums { get; set; }

        //Taxo
        public IList<string> MainStyles { get; private set; }
        public string MainStyle2 { get; private set; }
        public IList<string> StyleAlterations { get; private set; }
        public string ParsedStyle { get; private set; }
        public string UnparsedStyle { get; private set; }

        //Relations
        public IList<Disc> Discs { get; private set; }

        public int SortWeight { get { return SimilarAlbums.Count; } }

        public Album(int id, string title, DateTime releaseDate, int score, LabelVendor labelVendor, string coverPath
            , DateTime creationDate, Artist artist, Reviewer reviewer, IEnumerable<string> albumSimilarAlbums, string albumType)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            Score = score;
            LabelVendor = labelVendor;
            CoverPath = coverPath;
            AlbumType = albumType;
            Artist = artist;
            Reviewer = reviewer;
            var tmpSimilarAlbums = albumSimilarAlbums.Select(x => x.Replace("<i>", "").Replace("</i>", ""));
            RawSimilarAlbums = tmpSimilarAlbums.Where(x => x.ToUpperInvariant() != title.ToUpperInvariant()).Distinct().ToList();
            CreationDate = creationDate;
        }


        public string ToSQLInsert()
        {
            return string.Format("INSERT INTO `{0}` (`album_id`, `title`, `creation_date`, `release_date`, `reviewer_id`, `artist_id`, `score`, `label`, `cover_path`, `similar_albums_nodes`, `additionnal_similar_albums_titles` `sorting_weight`) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');"
                    , SQLTableName, Id, Title.ToCamelCase().GetSafeMySQL(), CreationDate.ToUnixTimeStamp(), ReleaseDate.ToUnixTimeStamp()
                    , Reviewer.Id, Artist.Id, Score, LabelVendor.Label.ToCamelCase(), CoverPath.GetSafeMySQL() 
                    , SimilarAlbums.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.Id).Trim('|')
                    , RawSimilarAlbums.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.ToCamelCase().GetSafeMySQL()).Trim('|')
                    , SortWeight);
        }

        public string ToRailsInsert()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} = {1}.new", RailsModelName, RailsModelName.ToCamelCase());
            sb.AppendLine();
            sb.AppendFormat("{0}.assign_attributes({{ id: {1}, title: '{2}', release_date: DateTime.parse('{3}'), kind_cd: {4}.kinds({5}), music_label_id: {6}, created_at: DateTime.parse('{7}'), updated_at: DateTime.parse('{7}'), artist_ids: [{8}], creator_id: {9}, updater_id: {9} }}, :without_protection => true)",
                RailsModelName, Id, Title.GetSafeRails(), ReleaseDate, RailsModelName.ToCamelCase(), AlbumType.ToRailsSym() ?? ":album", LabelVendor.Id, CreationDate, Artist.Id, Reviewer.Id);
            sb.AppendLine();
            sb.AppendFormat("{0}s << {0}", RailsModelName);
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }
    }
}