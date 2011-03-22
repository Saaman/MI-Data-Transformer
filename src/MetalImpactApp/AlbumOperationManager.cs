using System;
using System.Collections.Generic;
using MetalImpactApp.Operations;
using MIProgram.Core;
using MIProgram.Core.ProductStores;
using MIProgram.Core.Writers;
using MIProgram.Model;

namespace MetalImpactApp
{
    public class AlbumOperationManager : OperationManager<Album>
    {
        private readonly AlbumRepository _albumRepository;
        private readonly IDictionary<OperationType, IOperationProcessor<Album>> _operationsDefinitions;

        public AlbumOperationManager(ReviewProcessor<Album> reviewsProcessor, IList<Operation> operationsToProcess, IWriter writer, List<Func<Product, bool>> filtersDefinitions)
            : base(reviewsProcessor, operationsToProcess)
        {
            _operationsDefinitions = new Dictionary<OperationType, IOperationProcessor<Album>> { { OperationType.PublishAlbumCountries, new PublishArtistCountriesOperation(writer) } };

            _albumRepository = new AlbumRepository(filtersDefinitions);
        }

        protected override IDictionary<OperationType, IOperationProcessor<Album>> OperationsDefinition
        {
            get { return _operationsDefinitions; }
        }

        public override IProductRepository<Album> ProductRepository
        {
            get { return _albumRepository as IProductRepository<Album>; }
        }
    }
}