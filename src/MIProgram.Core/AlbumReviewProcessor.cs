using System;
using System.Collections.Generic;
using MIProgram.Core.AlbumImpl;
using MIProgram.Core.DataParsers;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Model;

namespace MIProgram.Core
{
    public class AlbumReviewProcessor : ReviewProcessor<Album>
    {
        private readonly CountryCodesParser _countryCodesParser = new CountryCodesParser();
        private readonly AlbumTypesParser _albumTypesParser = new AlbumTypesParser();

        public AlbumReviewProcessor(IMIRecordsProvider miRecordsProvider, IReviewExploder<Album> reviewExploder)
            : base(miRecordsProvider, reviewExploder)
        {}

        protected override void PostProcess()
        {
            throw new NotImplementedException();
        }

        protected override void SpecificProcess(IExplodedReview<Album> explodedReview)
        {
            //Parse Country
            ParseCountry(explodedReview);

            //Parse AlbumType
            ParseAlbumType(explodedReview);
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