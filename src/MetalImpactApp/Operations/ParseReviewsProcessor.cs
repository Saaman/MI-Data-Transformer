using System;
using System.Collections.Generic;
using System.ComponentModel;
using MIProgram.Core.Logging;
using MIProgram.WorkingModel;

namespace MetalImpactApp.Operations
{
    public class ParseReviewsProcessor : IOperationProcessor
    {
        internal static IList<Review> InternalProcess(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "Lecture des reviews...";
            worker.ReportProgress(0);
            var records = managerDeprecated.FilteredOriginalReviews;

            //Parse all reviews
            var count = 0;

            foreach (var record in records)
            {
                try
                {
                    managerDeprecated.AddReviewFrom(record);

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors du parsing de la review {0} : {1}", record.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = string.Format("Extraction des reviews... {0} sur {1}", ++count, records.Count);
                worker.ReportProgress(count * 100 / (records.Count));
            }

            return managerDeprecated.ParsedReviews;
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            InternalProcess(worker, e, managerDeprecated);
        }
    }
}