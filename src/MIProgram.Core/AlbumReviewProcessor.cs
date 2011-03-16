using System;
using MIProgram.Core.AlbumImpl;
using MIProgram.Core.DataParsers;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Model;

namespace MIProgram.Core
{
    public class AlbumReviewProcessor : ReviewProcessor<Album>
    {
        private readonly CountryCodesParser _countryCodesParser = new CountryCodesParser();

        public AlbumReviewProcessor(IMIRecordsProvider miRecordsProvider, IReviewExploder<Album> reviewExploder)
            : base(miRecordsProvider, reviewExploder)
        {

        }

        protected override void PostProcess()
        {
            throw new System.NotImplementedException();
        }

        protected override void SpecificProcess(IExplodedReview<Album> explodedReview)
        {
            //Parse Country
            ParseCountry(explodedReview);
        }

        private void ParseCountry(IExplodedReview<Album> explodedReview)
        {
            var review = explodedReview as AlbumExplodedReview;

            if (review == null)
            {
                throw new InvalidCastException("explodedReview cannot be cast as AlbumExplodedReview");
            }

            CountryDefinition countryDefinition = null;
            if (_countryCodesParser.TryParse(review.ArtistCountry, review.RecordId, ref countryDefinition))
            {
                //TODO : maybe good, see how to convert to a real Domain entity after in the AsDomainEntity
                review.ProcessedArtistCountry = countryDefinition;
            }
        }
    }
}