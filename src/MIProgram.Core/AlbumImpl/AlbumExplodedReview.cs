using System;
using System.Collections.Generic;
using MIProgram.Core.AlbumImpl.DataParsers;
using MIProgram.Core.Model;

namespace MIProgram.Core.AlbumImpl
{
    public class AlbumExplodedReview : IExplodedReview<Album>
    {
        public string RecordTitle { get; private set; }

        public string ReviewerName { get; private set; }
        public string ReviewerMail { get; private set; }
        
        public DateTime RecordCreationDate { get; private set; }
        public Album AsDomainEntity
        {
            get { throw new NotImplementedException(); }
        }

        public int RecordId { get; private set; }
        public DateTime RecordLastUpdateDate { get; private set; }

        public string ArtistName { get; private set; }
        public string ArtistCountry { get; private set; }
        public string ArtistOfficialUrl { get; private set; }
        public IList<string> ArtistSimilarArtists { get; private set; }
        
        public string AlbumName { get; private set; }
        public DateTime AlbumReleaseDate { get; private set; }
        public string AlbumLabel { get; private set; }
        public string AlbumDistributor { get; private set; }
        public string AlbumMusicGenre { get; private set; }
        public string AlbumType { get; private set; }
        public TimeSpan AlbumPlayTime { get; private set; }
        public string AlbumCoverFileName { get; private set; }
        public int AlbumSongsCount { get; private set; }
        public IList<string> AlbumSimilarAlbums { get; private set; }
        
        public int ReviewScore { get; private set; }
        public int ReviewHits { get; private set; }
        public string ReviewBody { get; private set; }
        
        public string DeezerAlbum { get; private set; }
        public string DeezerArtist { get; private set; }

        #region processed fields
        
        public IList<Country> ProcessedArtistCountries { get; set; }
        public string ProcessedAlbumType { get; set; }
        public LabelVendor ProcessedLabelVendor { get; set; }        
        public StyleDefinition ProcessedAlbumStyle { get; set; }

        #endregion

        public void CleanTextUsing(Func<IExplodedReview<Album>, string> cleanTextMethod)
        {
            ReviewBody = cleanTextMethod(this);
        }

        public AlbumExplodedReview(string reviewerName, string reviewerMail, DateTime recordCreationDate, string artistName, string artistCountry, string artistOfficialUrl, IList<string> artistSimilarArtists, string albumName, DateTime albumReleaseDate, string albumLabel, string albumDistributor, string albumMusicGenre, string albumType, TimeSpan albumPlayTime, string albumCoverFileName, int albumSongsCount, IList<string> albumSimilarAlbums, int recordId, int reviewScore, int reviewHits, string reviewBody, DateTime recordLastUpdateDate, string deezerAlbum, string deezerArtist, string recordTitle)
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
            RecordTitle = recordTitle;
        }
    }
}