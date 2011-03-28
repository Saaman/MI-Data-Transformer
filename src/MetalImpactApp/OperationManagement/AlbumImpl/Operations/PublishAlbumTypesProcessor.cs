using System;
using MIProgram.Core;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;
using MIProgram.Core.Writers;
using System.Linq;

namespace MetalImpactApp.OperationManagement.AlbumImpl.Operations
{
    public class PublishAlbumTypesProcessor : IOperationProcessor<Album>
    {
        private readonly IWriter _writer;

        public PublishAlbumTypesProcessor(IWriter writer)
        {
            _writer = writer;
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;
            var outputDir = Constants.FieldsExtractionsOutputDirectoryPath;

            if (albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            _writer.WriteTextCollection(albumRepository.ExplodedReviews.Select(x => string.Format("'{0}' est parsé en '{1}'", x.AlbumType, x.ProcessedAlbumType)).ToList(), "AlbumTypes", outputDir);
        }

        public string ProcessDescription
        {
            get { return "publication des types d'album parsés..."; }
        }
    }
}