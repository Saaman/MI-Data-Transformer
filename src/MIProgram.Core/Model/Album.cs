using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.Extensions;

namespace MIProgram.Core.Model
{
    public class Album : Product
    {
        public const string SQLTableName = "mi_album";

        //Existing
        public string Title { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public int Score { get; private set; }
        public string Label { get; private set; }
        public string ParsedType { get; private set; }
        public string UnParsedType { get; private set; }
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
            , DateTime creationDate, Artist artist, Reviewer reviewer, IList<string> albumSimilarAlbums)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            Score = score;
            LabelVendor = labelVendor;
            CoverPath = coverPath;
            Artist = artist;
            Reviewer = reviewer;
            var tmpSimilarAlbums = albumSimilarAlbums.Select(x => x.Replace("<i>", "").Replace("</i>", ""));
            RawSimilarAlbums = tmpSimilarAlbums.Where(x => x.ToUpperInvariant() != title.ToUpperInvariant()).Distinct().ToList();
            CreationDate = creationDate;
        }


        public string ToSQLInsert()
        {
            return string.Format("INSERT INTO `{0}` (`album_id`, `title`, `creation_date`, `release_date`, `reviewer_id`, `artist_id`, `score`, `label`, `cover_path`, `similar_albums_nodes`, `additionnal_similar_albums_titles` `sorting_weight`) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');"
                    , SQLTableName, Id, Title.ToCamelCase().GetSafeMySQL(), CreationDate.ToUnixTimeStamp(), ReleaseDate.ToUnixTimeStamp()
                    , Reviewer.Id, Artist.Id, Score, LabelVendor.Label.ToCamelCase(), CoverPath.GetSafeMySQL() 
                    , SimilarAlbums.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.Id).Trim('|')
                    , RawSimilarAlbums.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.ToCamelCase().GetSafeMySQL()).Trim('|')
                    , SortWeight);
        }
    }
}