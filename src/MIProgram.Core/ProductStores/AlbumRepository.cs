using System;
using System.Collections.Generic;
using MIProgram.Core.AlbumImpl;
using MIProgram.Model;

namespace MIProgram.Core.ProductStores
{
    public class AlbumRepository : ProductRepository<Album>
    {
        private readonly IList<Album> _albums = new List<Album>();
        private readonly IList<AlbumExplodedReview> _explodedReviews = new List<AlbumExplodedReview>();

        public AlbumRepository(List<Func<Product, bool>> filtersDefinitions) : base(filtersDefinitions)
        {}

        public override void Add(IExplodedReview<Album> explodedReview)
        {
            var review = explodedReview as AlbumExplodedReview;
            if (review == null)
            {
                throw new InvalidCastException("Cannot cast review as 'AlbumExplodedReview'");
            }

            _explodedReviews.Add(review);

            var reviewer = GetOrBuildReviewer(review.ReviewerName, review.ReviewerMail, review.RecordLastUpdateDate);

            var artist = GetOrBuildArtist(review.ArtistName, review.ProcessedArtistCountries, review.ArtistOfficialUrl,
                                          review.RecordLastUpdateDate, reviewer, review.ProcessedSimilarArtists);

            var album = new Album(review.AlbumName, DateTime.Parse(review.AlbumReleaseDate), review.ReviewScore, review.AlbumLabel, review.AlbumCoverFileName, artist);
            _albums.Add(album);
        }

        public override IList<Album> Products
        {
            get { return _albums; }
        }

        public IList<AlbumExplodedReview> ExplodedReviews
        {
            get { return _explodedReviews; }
        }
    }
}