using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml.Linq;
using MIProgram.Core.Creators;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;
using MIProgram.DataAccess;
using MIProgram.WorkingModel;

namespace MetalImpactApp
{
    public class DeezerOperationsProcessor
    {
        private readonly string _deezerDir;
        private readonly XMLCreator _xmlCreator;

        public DeezerOperationsProcessor(IWriter writer, string deezerDir)
        {
            _deezerDir = deezerDir;
            _xmlCreator = new XMLCreator(writer, deezerDir);
        }

        public void PublishSiteMap(BackgroundWorker worker, DoWorkEventArgs e, IList<MIDBRecord> records, OperationsManager manager)
        {
            manager.Infos = "Génération du sitemap...";
            var count = 0;
            worker.ReportProgress(count);

            var nodes = new List<XElement>();
            foreach (var record in records)
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

                manager.Infos = "Génération du sitemap... ";
                worker.ReportProgress(++count * 100 / (records.Count));
            }

            var doc = _xmlCreator.CreateSiteMap(nodes);
            _xmlCreator.Publish(doc, "SitemapCDreviews");
        }

        public void PublishLatestReviewsReport(BackgroundWorker worker, DoWorkEventArgs e, IList<Review> xmlReviewsToUpdate, OperationsManager manager)
        {
            manager.Infos = "Génération du fichier XML des reviews récentes...";
            int count = 0;
            worker.ReportProgress(count);

            var nodes = new List<XElement>();

            foreach (var review in xmlReviewsToUpdate)
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

                manager.Infos = "Génération du fichier XML des reviews récentes...";
                worker.ReportProgress(++count * 100 / (xmlReviewsToUpdate.Count));
            }

            var doc = _xmlCreator.CreateReport(nodes);
            _xmlCreator.Publish(doc, "LastReviews", _deezerDir);
        }

        public void PublishCompleteReviewsReport(BackgroundWorker worker, DoWorkEventArgs e, IList<Review> reviews, OperationsManager manager)
        {
            manager.Infos = "Génération du fichier complet des reviews...";
            var count = 0;
            worker.ReportProgress(count);

            var nodes = new List<XElement>();
            foreach (var review in reviews)
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
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de la génération du fichier complet des reviews (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                manager.Infos = "Génération du fichier complet des reviews... ";
                worker.ReportProgress(++count * 100 / (reviews.Count));
            }

            var doc = _xmlCreator.CreateReport(nodes);
            _xmlCreator.Publish(doc, "AllReviews", _deezerDir);
        }

        public IList<Review> PublishSinglesXMLs(BackgroundWorker worker, DoWorkEventArgs e, List<Review> xmlReviewsToUpdate, OperationsManager manager)
        {
            manager.Infos = "Génération des fichiers XML...";
            var count = 0;
            worker.ReportProgress(count);

            foreach (var review in xmlReviewsToUpdate)
            {
                try
                {
                    var xDoc = _xmlCreator.CreateSingleXML(review);
                    _xmlCreator.Publish(xDoc, review.Id.ToString(), ConfigurationManager.AppSettings["DeezerDir"]);

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

                manager.Infos = string.Format("Génération des fichiers XML... {0} sur {1}", ++count, xmlReviewsToUpdate.Count);
                worker.ReportProgress(count * 100 / (xmlReviewsToUpdate.Count));
            }

            return xmlReviewsToUpdate;
        }
    }
}