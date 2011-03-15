using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using MIProgram.Core.Creators;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;

namespace MetalImpactApp.Operations
{
    public class PublishDiffDeezerListingProcessor : IOperationProcessor
    {
        private readonly string _outputDir;
        private readonly XMLCreator _xmlCreator;

        public PublishDiffDeezerListingProcessor(IWriter writer, string outputDir)
        {
            _outputDir = outputDir;
            _xmlCreator = new XMLCreator(writer, outputDir);
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "Génération du fichier XML des reviews récentes...";
            int count = 0;
            worker.ReportProgress(count);

            var nodes = new List<XElement>();

            foreach (var review in managerDeprecated.ParsedReviewsToUpdate)
            {
                try
                {
                    nodes.Add(_xmlCreator.GetXmlForReport(review.Id));

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération du fichier XML des reviews récentes (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = "Génération du fichier XML des reviews récentes...";
                worker.ReportProgress(++count * 100 / (managerDeprecated.ParsedReviewsToUpdate.Count));
            }

            var doc = _xmlCreator.CreateReport(nodes);
            _xmlCreator.Publish(doc, "LastReviews", _outputDir);
        }
    }
}