using System;
using System.Collections.Generic;
using MetalImpactApp.OperationManagement.AlbumImpl.Operations;
using MIProgram.Core;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;
using MIProgram.Core.Writers;

namespace MetalImpactApp.OperationManagement.AlbumImpl
{
    public class AlbumOperationManager : OperationManager<Album>
    {
        private readonly AlbumRepository _albumRepository;
        private readonly IDictionary<OperationType, IOperationProcessor<Album>> _operationsDefinitions;

        public AlbumOperationManager(ReviewProcessor<Album> reviewsProcessor, IList<Operation> operationsToProcess, IWriter writer, List<Func<Product, bool>> filtersDefinitions, DateTime lastExportDate)
            : base(reviewsProcessor, operationsToProcess)
        {
            _operationsDefinitions = new Dictionary<OperationType, IOperationProcessor<Album>>
                                         {
                                             { OperationType.PublishAlbumCountries, new PublishArtistCountriesProcessor(writer) },
                                             { OperationType.PublishAlbumTypes, new PublishAlbumTypesProcessor(writer) },
                                             { OperationType.PublishMusicStyles, new PublishMusicStylesProcessor(writer)},
                                             { OperationType.PublishDiffDeezerListing, new PublishDiffDeezerListingProcessor(writer)},
                                             { OperationType.PublishFullDeezerListing, new PublishFullDeezerListingProcessor(writer)},
                                             { OperationType.PublishReviewsWithNewModel, new PublishReviewsWithNewModelProcessor(writer)},
                                             { OperationType.PublishReviewsXMLsForDeezer, new PublishReviewsXMLsForDeezerProcessor(writer)},
                                             { OperationType.PublishSiteMap, new PublishSiteMapProcessor(writer)}
                                         };

            _albumRepository = new AlbumRepository(filtersDefinitions, lastExportDate);
        }

        protected override IDictionary<OperationType, IOperationProcessor<Album>> OperationsDefinition
        {
            get { return _operationsDefinitions; }
        }

        public override ProductRepository<Album> ProductRepository
        {
            get { return _albumRepository; }
        }
    }
}