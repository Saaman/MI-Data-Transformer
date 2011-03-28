using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MIProgram.Core;
using MIProgram.Core.Creators;
using MIProgram.Core.Logging;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;
using MIProgram.Core.Writers;

namespace MetalImpactApp.OperationManagement.AlbumImpl.Operations
{
    public class PublishFullDeezerListingProcessor : IOperationProcessor<Album>
    {
        private readonly XMLCreator _xmlCreator;

        public PublishFullDeezerListingProcessor(IWriter writer)
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

            var nodes = new List<XElement>();
            foreach (var review in albumRepository.ExplodedReviews)
            {
                try
                {
                    nodes.Add(_xmlCreator.GetXmlForReport(review.RecordId));
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération du fichier complet des reviews (review {0}) : {1}", review.RecordId, ex.Message), ErrorLevel.Error);
                    continue;
                }
            }

            var doc = _xmlCreator.CreateReport(nodes);
            _xmlCreator.Publish(doc, "AllReviews");
        }

        public string ProcessDescription
        {
            get { return "Génération du fichier complet des reviews... "; }
        }
    }
}