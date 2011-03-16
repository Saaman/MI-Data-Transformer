using System.Collections.Generic;
using MIProgram.Core;
using MIProgram.Core.ProductStores;
using MIProgram.Core.Writers;
using MIProgram.Model;

namespace MetalImpactApp
{
    public class AlbumOperationManager : IOperationManager<Album>
    {
        private readonly AlbumRepository _albumRepository = new AlbumRepository();
        private readonly IDictionary<OperationType, IOperationProcessor<Album>> _operationsDefinitions;

        public AlbumOperationManager(ReviewProcessor<Album> reviewsProcessor, IList<Operation> operationsToProcess, IWriter writer) : base(reviewsProcessor, operationsToProcess)
        {
            _operationsDefinitions = new Dictionary<OperationType, IOperationProcessor<Album>> { { OperationType.PublishAlbumCountries, new PublishArtistCountriesOperation(writer) } };
        }

        protected override IDictionary<OperationType, IOperationProcessor<Album>> OperationsDefinition
        {
            get { return _operationsDefinitions; }
        }

        public override IProductRepository<Album> ProductRepository
        {
            get { return _albumRepository; }
        }
    }
}