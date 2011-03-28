using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.AlbumImpl.DataParsers;
using MIProgram.Core.AlbumImpl.DataParsers.TreeBuilder;
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

        public AlbumReviewProcessor(IMIRecordsProvider miRecordsProvider, IReviewExploder<Album> reviewExploder)
            : base(miRecordsProvider, reviewExploder)
        {}

        protected override void PostProcess(IList<IExplodedReview<Album>> explodedReviews)
        {
            var stylesDefinitions =
                explodedReviews.ToDictionary(x => ((AlbumExplodedReview) x).AlbumMusicGenre,
                                             x => ((AlbumExplodedReview) x).ProcessedAlbumStyle).OrderBy(
                    x => x.Value.Complexity).ToDictionary(x => x.Key, y => y.Value);
            _stylesTree = StylesTree.BuildFrom(stylesDefinitions);

            /*foreach (var artist in newArtists)
            {
                var currentArtist = artist;
                currentArtist.SimilarArtists = newArtists.Where(
                    x => currentArtist.ArtistSimilarArtistsNames.Contains(x.Name, new UpperInvariantComparer())
                        && x != currentArtist).ToList();
                currentArtist.ArtistSimilarArtistsNames = currentArtist.ArtistSimilarArtistsNames.Where(
                    y => !currentArtist.SimilarArtists.Select(x => x.Name).Contains(y, new UpperInvariantComparer())).ToList();
            }*/
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

        protected override void FinalizeProductRepository(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;

            if(albumRepository == null)
            {
                throw new InvalidCastException("productRepository cannot be cast as AlbumRepository");
            }

            albumRepository.AttachStylesTree(_stylesTree);

        }

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
            if (_albumTypesParser.TryParse(review.ArtistCountry, review.RecordId, ref albumType))
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
    }
}