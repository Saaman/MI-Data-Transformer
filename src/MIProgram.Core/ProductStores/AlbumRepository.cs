using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.AlbumImpl;
using MIProgram.Model;

namespace MIProgram.Core.ProductStores
{
    public class AlbumRepository : IProductRepository<Album>
    {
        private readonly IList<Artist> _artists = new List<Artist>();
        private readonly IList<Reviewer> _reviewers = new List<Reviewer>();
        private readonly IList<Album> _albums = new List<Album>();

        /*Id generators*/
        private readonly IDGenerator _artistIdGenerator = new IDGenerator();
        private readonly IDGenerator _reviewerIdGenerator = new IDGenerator();

        #region IProductRepository Implementation

        private Artist GetOrBuildArtist(string artistName, IList<Country> countries, string officialUrl, DateTime lastUpdate, Reviewer reviewer, IList<Artist> similarArtists)
        {
            var selectedArtist = (from artist in Artists where artist.Name == artistName.ToUpperInvariant() select artist).SingleOrDefault();

            if (selectedArtist == null)
            {
                selectedArtist = new Artist(_artistIdGenerator.NewID(), artistName, countries, officialUrl, lastUpdate, reviewer, similarArtists);
                Artists.Add(selectedArtist);
                return selectedArtist;
            }

            selectedArtist.UpdateInfos(countries, officialUrl, lastUpdate, reviewer, similarArtists);
            
            return selectedArtist;
        }

        private Reviewer GetOrBuildReviewer(string name, string mailAddress, DateTime lastUpdate)
        {
            var selectedReviewer = (from reviewer in Reviewers where reviewer.Name == name select reviewer).SingleOrDefault();

            if (selectedReviewer == null)
            {
                selectedReviewer = new Reviewer(_reviewerIdGenerator.NewID(), name, mailAddress, lastUpdate);
                Reviewers.Add(selectedReviewer);
                return selectedReviewer;
            }

            if (selectedReviewer.LastUpdate < lastUpdate)
            {
                selectedReviewer.UpdateInfos(name, mailAddress, lastUpdate);
            }

            return selectedReviewer;
        }

        
        public IList<Artist> Artists
        {
            get { return _artists; }
        }

        public IList<Reviewer> Reviewers
        {
            get { return _reviewers; }
        }

        public IList<Album> Products
        {
            get { return _albums; }
        }

        public void Add(IExplodedReview<Album> explodedReview)
        {
            var review = explodedReview as AlbumExplodedReview;
            if(review == null)
            {
                throw new InvalidCastException("Cannot cast review as 'AlbumExplodedReview'");
            }

            var reviewer = GetOrBuildReviewer(review.ReviewerName, review.ReviewerMail, review.RecordLastUpdateDate);

            var artist = GetOrBuildArtist(review.ArtistName, review.ArtistParsedCountries, review.ArtistOfficialUrl,
                                          review.RecordLastUpdateDate, reviewer, review.ArtistParsedSimilarArtists);

            var album = new Album(review.AlbumName, DateTime.Parse(review.AlbumReleaseDate), review.ReviewScore, review.AlbumLabel, review.AlbumCoverFileName, artist);
            _albums.Add(album);

        }

        #endregion
    }
}