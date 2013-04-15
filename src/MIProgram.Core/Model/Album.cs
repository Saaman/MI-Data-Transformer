using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIProgram.Core.AlbumImpl.DataParsers;
using MIProgram.Core.Extensions;
using MIProgram.Core.Logging;

namespace MIProgram.Core.Model
{
    public class Album : Product
    {
        public const string SQLTableName = "mi_album";
        public const string RailsModelName = "album";
        private const string CoversRootUri = "http://www.metal-impact.com/modules/Reviews/images/";

        //Existing
        public DateTime ReleaseDate { get; private set; }
        public int Score { get; private set; }
        public string Label { get; private set; }
        public string AlbumType { get; private set; }
        public string RawAlbumMusicGenre { get; private set; }
        public StyleDefinition ProcessedAlbumMusicGenre { get; private set; }
        public string CoverName { get; private set; }

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

        public Review Review { get; private set; }

        public int SortWeight { get { return SimilarAlbums.Count; } }

        public Album(int id, string title, DateTime releaseDate, int score, LabelVendor labelVendor, string coverName, DateTime creationDate, Artist artist, Reviewer reviewer, IEnumerable<string> albumSimilarAlbums, string albumType, string rawAlbumMusicGenre, StyleDefinition processedAlbumMusicGenre, Review chronique)
        {
            Review = chronique;
            Review.Product = this;

            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            Score = score;
            LabelVendor = labelVendor;
            CoverName = coverName;
            AlbumType = albumType;
            RawAlbumMusicGenre = rawAlbumMusicGenre;
            ProcessedAlbumMusicGenre = processedAlbumMusicGenre;
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
                    , Reviewer.Id, Artist.Id, Score, LabelVendor.Label.ToCamelCase(), CoverName.GetSafeMySQL() 
                    , SimilarAlbums.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.Id).Trim('|')
                    , RawSimilarAlbums.Aggregate(string.Empty, (seed, entry) => seed + "|" + entry.ToCamelCase().GetSafeMySQL()).Trim('|')
                    , SortWeight);
        }

        public string ToRailsInsert()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} = {1}.new", RailsModelName, RailsModelName.ToCamelCase());
            sb.AppendLine();
            var convertedAlbumType = string.IsNullOrEmpty(AlbumType) ? string.Empty : AlbumType.ToRailsSym();
            var allowedAlbumTypes = new List<string> {":album", ":demo", ":mini_album", ":live"};
            if (!allowedAlbumTypes.Contains(convertedAlbumType))
            {
                Logging.Logging.Instance.LogError(string.Format("'{0}' is not recognized as a valid album type for album {1}-{2}. value is fallbacked to ':album'", AlbumType, Id, Title), ErrorLevel.Warning);
                convertedAlbumType = ":album";
            }
            sb.AppendFormat("{0}.assign_attributes({{ id: {1}, title: '{2}', release_date: DateTime.parse('{3}'), kind_cd: {4}.kinds({5}), music_label_id: {6}, created_at: DateTime.parse('{7}'), updated_at: DateTime.parse('{7}'), artist_ids: [{8}], creator_id: {9}, updater_id: {9}, published: true, remote_cover_url: \"{10}\" }}, :without_protection => true)",
                RailsModelName, Id, Title.GetSafeRails(), ReleaseDate, RailsModelName.ToCamelCase(), convertedAlbumType, LabelVendor.Id, CreationDate, Artist.Id, Reviewer.Id, CoversRootUri + CoverName);
            sb.AppendLine();
            sb.AppendFormat("{0}s << {0}", RailsModelName);
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }

        public string ToYAMLInsert()
        {
            var convertedAlbumType = string.IsNullOrEmpty(AlbumType) ? string.Empty : AlbumType.ToRailsSym();
            var allowedAlbumTypes = new List<string> {":album", ":demo", ":mini_album", ":live"};
            if (!allowedAlbumTypes.Contains(convertedAlbumType))
            {
                Logging.Logging.Instance.LogError(string.Format("'{0}' is not recognized as a valid album type for album {1}-{2}. value is fallbacked to ':album'", AlbumType, Id, Title), ErrorLevel.Warning);
                convertedAlbumType = ":album";
            }

            var sb = new StringBuilder();
            sb.AppendFormat("id: {0}", Id);
            sb.AppendLine();
            sb.AppendLine("model: album");
            sb.AppendFormat("title: '{0}'", Title.GetSafeRails());
            sb.AppendLine();
            sb.AppendFormat("music_label_id: {0}", LabelVendor.Id);
            sb.AppendLine();
            sb.AppendFormat("artist_ids: [{0}]", Artist.Id);
            sb.AppendLine();
            sb.AppendFormat("cover: '{0}'", CoversRootUri + CoverName);
            sb.AppendLine();
            sb.AppendFormat("album_type: {0}", convertedAlbumType);
            sb.AppendLine();
            sb.AppendFormat("music_genre: \"{0}\"", RawAlbumMusicGenre);
            sb.AppendLine();
            if (ProcessedAlbumMusicGenre != null)
            {
                sb.AppendFormat("music_genre_music_types: [{0}]",
                                (ProcessedAlbumMusicGenre.MusicTypes.Count() == 0)? ""
                                    : ProcessedAlbumMusicGenre.MusicTypes.Select(val => string.Format("\"{0}\"", val)).
                                          Aggregate((acc, next) => acc + ", " + next));
                sb.AppendLine();
                sb.AppendFormat("music_genre_main_styles: [{0}]",
                                (ProcessedAlbumMusicGenre.MainStyles.Count() == 0)? ""
                                    : ProcessedAlbumMusicGenre.MainStyles.Select(val => string.Format("\"{0}\"", val)).
                                          Aggregate((acc, next) => acc + ", " + next));
                sb.AppendLine();
                sb.AppendFormat("music_genre_style_alterations: [{0}]",
                                (ProcessedAlbumMusicGenre.StyleAlterations.Count() == 0)? ""
                                    : ProcessedAlbumMusicGenre.StyleAlterations.Select(val => string.Format("\"{0}\"", val)).
                                            Aggregate((acc, next) => acc + ", " + next));
                sb.AppendLine();
            }
            sb.AppendFormat("release_date: {0}", ReleaseDate);
            sb.AppendLine();
            sb.AppendFormat("created_at: {0}", CreationDate);
            sb.AppendLine();
            sb.AppendFormat("updated_at: {0}", CreationDate);
            sb.AppendLine();
            sb.AppendFormat("created_by: {0}", Reviewer.Id);
            sb.AppendLine();
            return sb.ToString();
        }
    }
}