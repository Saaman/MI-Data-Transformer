using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.AlbumImpl.DataParsers;
using MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder;
using MIProgram.Core.Extensions;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;

namespace MIProgram.Core.AlbumImpl
{
    public class AlbumReviewProcessor : ReviewProcessor<Album>
    {
        private readonly CountryCodesParser _countryCodesParser = new CountryCodesParser();
        private readonly AlbumTypesParser _albumTypesParser = new AlbumTypesParser();
        private readonly AlbumStylesParser _albumStylesParser = new AlbumStylesParser();
        private StylesTree _stylesTree;

        public AlbumReviewProcessor(IMIRecordsProvider miRecordsProvider, IReviewExploder<Album> reviewExploder, ICanShowReviewCleaningForm iCanShowReviewCleaningForm)
            : base(miRecordsProvider, reviewExploder, new AlbumReviewBodyCleaner(iCanShowReviewCleaningForm))
        {}

        protected override void PostProcessExplodedReviews(IList<IExplodedReview<Album>> explodedReviews)
        {
            var stylesDefinitions = new Dictionary<string, StyleDefinition>();
            foreach (var explodedReview in explodedReviews)
            {
                var albumReview = explodedReview as AlbumExplodedReview;
                if(albumReview == null)
                {
                    throw new InvalidCastException("explodedReview cannot be cast as AlbumExplodedReview");
                }

                if (albumReview.ProcessedAlbumStyle == null)
                {
                    continue;
                }

                if(!stylesDefinitions.ContainsKey(albumReview.AlbumMusicGenre.ToUpperInvariant()))
                {
                    stylesDefinitions.Add(albumReview.AlbumMusicGenre.ToUpperInvariant(), albumReview.ProcessedAlbumStyle);
                }
            }

            _stylesTree = StylesTree.BuildFrom(stylesDefinitions);
        }

        protected override void SpecificProcess(IExplodedReview<Album> explodedReview)
        {
            //Parse Country
            ParseCountry(explodedReview);

            //Parse AlbumType
            ParseAlbumType(explodedReview);

            //Parse Style
            ParseAlbumStyle(explodedReview);
        }

        protected override void PostProcessProductRepository(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;

            if(albumRepository == null)
            {
                throw new InvalidCastException("productRepository cannot be cast as AlbumRepository");
            }

            albumRepository.AttachStylesTree(_stylesTree);

            //Build links between artists
            foreach (var artist in productRepository.Artists)
            {
                var currentArtist = artist;
                currentArtist.SimilarArtists = productRepository.Artists.Where(
                    x => currentArtist.RawSimilarArtists.Contains(x.Name, new UpperInvariantComparer())
                        && x != currentArtist).ToList();
            }

            //Build links between albums
            foreach (var album in productRepository.Products)
            {
                var currentAlbum = album;
                currentAlbum.SimilarAlbums = productRepository.Products.Where(
                    x => currentAlbum.RawSimilarAlbums.Contains(x.Title, new UpperInvariantComparer())
                        && x != currentAlbum).ToList();
            }
        }

        #region private methods

        private void ParseAlbumStyle(IExplodedReview<Album> explodedReview)
        {
            var review = explodedReview as AlbumExplodedReview;

            if (review == null)
            {
                throw new InvalidCastException("explodedReview cannot be cast as AlbumExplodedReview");
            }

            StyleDefinition albumStyle = null;
            if (_albumStylesParser.TryParse(review.AlbumMusicGenre, review.RecordId, ref albumStyle))
            {
                review.ProcessedAlbumStyle = albumStyle;
            }
        }

        private void ParseAlbumType(IExplodedReview<Album> explodedReview)
        {
            var review = explodedReview as AlbumExplodedReview;

            if (review == null)
            {
                throw new InvalidCastException("explodedReview cannot be cast as AlbumExplodedReview");
            }

            string albumType = null;
            if (_albumTypesParser.TryParse(review.AlbumType, review.RecordId, ref albumType))
            {
                review.ProcessedAlbumType = albumType;
            }
        }

        private void ParseCountry(IExplodedReview<Album> explodedReview)
        {
            var review = explodedReview as AlbumExplodedReview;

            if (review == null)
            {
                throw new InvalidCastException("explodedReview cannot be cast as AlbumExplodedReview");
            }

            IList<Country> countries = null;
            if (_countryCodesParser.TryParse(review.ArtistCountry, review.RecordId, ref countries))
            {
                review.ProcessedArtistCountries = countries;
            }
        }

        #endregion
    }
}