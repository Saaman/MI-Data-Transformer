using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MIProgram.Core;
using MIProgram.Core.DAL.Writers;
using MIProgram.Core.Logging;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;

namespace MetalImpactApp.OperationManagement.AlbumImpl.Operations
{
    public class PublishSiteMapProcessor : IOperationProcessor<Album>
    {
        private readonly XMLCreator _xmlCreator;

        public PublishSiteMapProcessor(IWriter writer)
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
            foreach (var explodedReview in albumRepository.ExplodedReviews)
            {
                try
                {
                    nodes.Add(_xmlCreator.GetXmlForSitemap(explodedReview.RecordLastUpdateDate, explodedReview.RecordId));
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération du sitemap (review {0}) : {1}", explodedReview.RecordId, ex.Message), ErrorLevel.Error);
                    continue;
                }
            }

            var doc = _xmlCreator.CreateSiteMap(nodes);
            _xmlCreator.Publish(doc, "SitemapCDreviews");
        }

        public string ProcessDescription
        {
            get { return "Génération du sitemap... "; }
        }
    }
}