using System;
using System.ComponentModel;
using MIProgram.Core.Creators;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;

namespace MetalImpactApp.Operations
{
    public class PublishReviewsXMLsForDeezerProcessor : IOperationProcessor
    {
        private readonly string _outputDir;
        private readonly XMLCreator _xmlCreator;

        public PublishReviewsXMLsForDeezerProcessor(IWriter writer, string outputDir)
        {
            _outputDir = outputDir;
            _xmlCreator = new XMLCreator(writer, outputDir);
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "Génération des fichiers XML...";
            var count = 0;
            worker.ReportProgress(count);
            
            foreach (var review in managerDeprecated.ParsedReviewsToUpdate)
            {
                try
                {
                    var xDoc = _xmlCreator.CreateSingleXML(review);
                    _xmlCreator.Publish(xDoc, review.Id.ToString(), _outputDir);

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la publication du XML de la review {0} : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = string.Format("Génération des fichiers XML... {0} sur {1}", ++count, managerDeprecated.ParsedReviewsToUpdate.Count);
                worker.ReportProgress(count * 100 / (managerDeprecated.ParsedReviewsToUpdate.Count));
            }
        }
    }
}