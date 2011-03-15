using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using MIProgram.Core.Creators;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;

namespace MetalImpactApp.Operations
{
    public class PublishSiteMapProcessor : IOperationProcessor
    {
        private readonly XMLCreator _xmlCreator;

        public PublishSiteMapProcessor(IWriter writer, string outputDir)
        {
            _xmlCreator = new XMLCreator(writer, outputDir);
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "Génération du sitemap...";
            var count = 0;
            worker.ReportProgress(count);

            var nodes = new List<XElement>();
            foreach (var record in managerDeprecated.AllOriginalReviews)
            {
                try
                {
                    nodes.Add(_xmlCreator.GetXmlForSitemap(record.LastUpdateDate ?? record.CreationDate, record.Id));

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération du sitemap (review {0}) : {1}", record.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = "Génération du sitemap... ";
                worker.ReportProgress(++count * 100 / (managerDeprecated.AllOriginalReviews.Count));
            }

            var doc = _xmlCreator.CreateSiteMap(nodes);
            _xmlCreator.Publish(doc, "SitemapCDreviews");
        }
    }
}