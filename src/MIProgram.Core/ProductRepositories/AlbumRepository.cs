using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.AlbumImpl;
using MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder;
using MIProgram.Core.Helpers;
using MIProgram.Core.Model;

namespace MIProgram.Core.ProductRepositories
{
    public class AlbumRepository : ProductRepository<Album>
    {
        private readonly IList<Album> _albums = new List<Album>();
        private readonly IList<AlbumExplodedReview> _explodedReviews = new List<AlbumExplodedReview>();
        private StylesTree _stylesTree;
        private readonly DateTime _lastExportDate;

        protected readonly IDGenerator _albumIdGenerator = new IDGenerator();

        public AlbumRepository(List<Func<Product, bool>> filtersDefinitions, DateTime lastExportDate) : base(filtersDefinitions)
        {
            _lastExportDate = lastExportDate;
        }

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
                                          review.RecordLastUpdateDate, reviewer, review.ArtistSimilarArtists);

            var chronique = new Review(review.ReviewBody, review.ReviewHits, review.ReviewScore, reviewer);

            var album = new Album(_albumIdGenerator.NewID(), review.AlbumName, review.AlbumReleaseDate, review.ReviewScore
                , review.ProcessedLabelVendor, review.AlbumCoverFileName, review.RecordLastUpdateDate, artist, reviewer, review.AlbumSimilarAlbums, review.ProcessedAlbumType
                , review.AlbumMusicGenre, review.ProcessedAlbumStyle, chronique);
            _albums.Add(album);
        }

        public void AttachStylesTree(StylesTree stylesTree)
        {
            _stylesTree = stylesTree;
        }

        public override IList<Album> Products
        {
            get { return _albums; }
        }

        public IList<AlbumExplodedReview> ExplodedReviews
        {
            get { return _explodedReviews; }
        }

        public IList<AlbumExplodedReview> LatestExplodedReviews
        { get { return _explodedReviews.Where(x => x.RecordLastUpdateDate > _lastExportDate).ToList(); } }

        public StylesTree StylesTree
        {
            get { return _stylesTree; }
        }
    }
}