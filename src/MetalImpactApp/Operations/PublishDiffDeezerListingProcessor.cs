using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using MIProgram.Core;
using MIProgram.Core.Creators;
using MIProgram.Core.Logging;
using MIProgram.Core.ProductStores;
using MIProgram.Core.Writers;
using MIProgram.Model;

namespace MetalImpactApp.Operations
{
    public class PublishDiffDeezerListingProcessor : IOperationProcessor<Album>
    {
        private readonly XMLCreator _xmlCreator;

        public PublishDiffDeezerListingProcessor(IWriter writer)
        {
            _xmlCreator = new XMLCreator(writer, Constants.DeezerOperationsOutputDirectoryPath);
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;
            var outputDir = Constants.DeezerOperationsOutputDirectoryPath;

            if (albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            var nodes = new List<XElement>();

            foreach (var review in albumRepository.LatestExplodedReviews)
            {
                nodes.Add(_xmlCreator.GetXmlForReport(review.RecordId));
            }

            var doc = _xmlCreator.CreateReport(nodes);
            _xmlCreator.Publish(doc, "LastReviews", outputDir);
        }

        public string ProcessDescription
        {
            get { return "Génération du fichier XML des reviews récentes..."; }
        }
    }
}