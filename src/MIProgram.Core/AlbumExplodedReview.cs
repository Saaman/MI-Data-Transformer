using System;
using System.Collections.Generic;
using MIProgram.Model;

namespace MIProgram.Core
{
    public class AlbumExplodedReview : IExplodedReview<Album>
    {
        public string ReviewerName { get; set; }
        public string ReviewerMail { get; set; }
        
        public DateTime RecordCreationDate { get; set; }
        public int RecordId { get; set; }
        public DateTime RecordLastUpdateDate { get; set; }

        public string ArtistName { get; set; }
        public string ArtistCountry { get; set; }
        public string ArtistOfficialUrl { get; set; }
        public IList<string> ArtistSimilarArtists { get; set; }
        
        public string AlbumName { get; set; }
        public string AlbumReleaseDate { get; set; }
        public string AlbumLabel { get; set; }
        public string AlbumDistributor { get; set; }
        public string AlbumMusicGenre { get; set; }
        public string AlbumType { get; set; }
        public TimeSpan AlbumPlayTime { get; set; }
        public string AlbumCoverFileName { get; set; }
        public int AlbumSongsCount { get; set; }
        public IList<string> AlbumSimilarAlbums { get; set; }
        
        public int ReviewScore { get; set; }
        public int ReviewHits { get; set; }
        public string ReviewBody { get; set; }
        
        public string DeezerAlbum { get; set; }
        public string DeezerArtist { get; set; }

        public AlbumExplodedReview(string reviewerName, string reviewerMail, DateTime recordCreationDate, string artistName, string artistCountry, string artistOfficialUrl, IList<string> artistSimilarArtists, string albumName, string albumReleaseDate, string albumLabel, string albumDistributor, string albumMusicGenre, string albumType, TimeSpan albumPlayTime, string albumCoverFileName, int albumSongsCount, IList<string> albumSimilarAlbums, int recordId, int reviewScore, int reviewHits, string reviewBody, DateTime recordLastUpdateDate, string deezerAlbum, string deezerArtist)
        {
            ReviewerName = reviewerName;
            ReviewerMail = reviewerMail;
            RecordCreationDate = recordCreationDate;
            ArtistName = artistName;
            ArtistCountry = artistCountry;
            ArtistOfficialUrl = artistOfficialUrl;
            ArtistSimilarArtists = artistSimilarArtists;
            AlbumName = albumName;
            AlbumReleaseDate = albumReleaseDate;
            AlbumLabel = albumLabel;
            AlbumDistributor = albumDistributor;
            AlbumMusicGenre = albumMusicGenre;
            AlbumType = albumType;
            AlbumPlayTime = albumPlayTime;
            AlbumCoverFileName = albumCoverFileName;
            AlbumSongsCount = albumSongsCount;
            AlbumSimilarAlbums = albumSimilarAlbums;
            RecordId = recordId;
            ReviewScore = reviewScore;
            ReviewHits = reviewHits;
            ReviewBody = reviewBody;
            RecordLastUpdateDate = recordLastUpdateDate;
            DeezerAlbum = deezerAlbum;
            DeezerArtist = deezerArtist;
        }
    }
}