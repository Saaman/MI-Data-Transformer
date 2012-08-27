using System;
using MIProgram.Core;
using MIProgram.Core.DAL.Writers;
using MIProgram.Core.Logging;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;

namespace MetalImpactApp.OperationManagement.AlbumImpl.Operations
{
    public class PublishReviewsXMLsForDeezerProcessor : IOperationProcessor<Album>
    {
        private readonly XMLCreator _xmlCreator;

        public PublishReviewsXMLsForDeezerProcessor(IWriter writer)
        {
            _xmlCreator = new XMLCreator(writer, Constants.DeezerOperationsOutputDirectoryPath);
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;

            if (albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            foreach (var product in albumRepository.FilteredProducts)
            {
                try
                {
                    var xDoc = _xmlCreator.CreateSingleXML(product);
                    _xmlCreator.Publish(xDoc, product.Id.ToString());
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la publication du XML de la review {0} : {1}", product.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }
            }
        }

        public string ProcessDescription
        {
            get { return "Génération des fichiers XML pour Deezer..."; }
        }
    }
}