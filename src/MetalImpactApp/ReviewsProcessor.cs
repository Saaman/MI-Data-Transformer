using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using MIProgram.Core;
using MIProgram.Core.Logging;
using MIProgram.Core.Cleaners;
using System.IO;
using MIProgram.Core.MIRecordsProviders;
using System.Reflection;
using System.Configuration;
using MIProgram.Core.Writers;
using MIProgram.DataAccess;
using System.Diagnostics;

namespace MetalImpactApp
{
    public class ReviewsProcessor : ICanShowReviewCleaningForm
    {
        public bool IsWorking { get; set; }
        public string Infos { get; set; }

        private readonly IMIRecordsProvider _miRecordsProvider;
        private readonly IWriter _writer;
        private readonly ReviewsManager _reviewsManager;
        private readonly DateTime _lastUpdateDate;
        private readonly IList<Operation> _operationsToPerform;
        private readonly DeezerOperationsProcessor _deezerOperationsProcessor;
        private readonly MigrationOperationsProcessor _migrationOperationsProcessor;

        public ReviewsProcessor(IMIRecordsProvider miRecordsProvider, IWriter writer, DateTime lastUpdateDate, IList<Operation> operationsToPerform)
        {
            _miRecordsProvider = miRecordsProvider;
            _writer = writer;
            _lastUpdateDate = lastUpdateDate;
            _operationsToPerform = operationsToPerform;

            var miRemovalsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "miRemovals.mi");
            var mainCleaner = FileReviewsCleaner.GetReviewCleanerFromFile(miRemovalsPath);
            var temporaryCleaner = new InMemoryCleaner();
            var reviewBuilder = new ReviewBuilder(mainCleaner, temporaryCleaner, this);
            _reviewsManager = new ReviewsManager(reviewBuilder);

            _deezerOperationsProcessor = new DeezerOperationsProcessor(_writer, ConfigurationManager.AppSettings["DeezerDir"]);
            _migrationOperationsProcessor = new MigrationOperationsProcessor(_writer, ConfigurationManager.AppSettings["MigrationDir"]);

            IsWorking = false;
        }

        public void EndWork()
        {
            IsWorking = false;
            _reviewsManager.FinalizeWork();
        }

        public long Process(BackgroundWorker worker, DoWorkEventArgs e)
        {
            IsWorking = true;
            
            Infos = "Lecture des reviews...";
            worker.ReportProgress(0);
            var results = _miRecordsProvider.GetRecords();

            //Parse all reviews
            ExtractReviewsInfos(worker, e, results);

            #region Migration

            //Publication des styles
            if (_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateMusicStyles).Any())
            {
                _migrationOperationsProcessor.ExtractMusicStyles(worker, e, _reviewsManager.Reviews, this);
            }

            //Publication des types d'album
            if (_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateRecordTypes).Any())
            {
                _migrationOperationsProcessor.ExtractAlbumTypes(worker, e, _reviewsManager.Reviews, this);
            }

            //Publication des pays d'artistes
            if (_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateCountries).Any())
            {
                _migrationOperationsProcessor.ExtractCountries(worker, e, _reviewsManager.Reviews, this);
            }

            //Publication des reviews avec le nouveau modèle
            if (_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateReviewsWithNewModel).Any())
            {
                _migrationOperationsProcessor.PublishReviewsWithNewModel(worker, e, _reviewsManager.Reviews, this);
            }

            #endregion

            #region Deezer

            //Publication des xmls

            var xmlReviewsToUpdate = _reviewsManager.Reviews.Where(x => x.LastUpdateDate > _lastUpdateDate).ToList();

            if (_operationsToPerform.Where(op => op.OperationType == OperationType.PublishReviews).Any())
            {
                _deezerOperationsProcessor.PublishSinglesXMLs(worker, e, xmlReviewsToUpdate, this);
            }

            if (xmlReviewsToUpdate.Count == 0)
            {
                return 0;
            }

            //Xml listant toutes les chroniques
            if(_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateFullDeezer).Any())
            {
                _deezerOperationsProcessor.PublishCompleteReviewsReport(worker, e, _reviewsManager.Reviews, this);
            }

            //Xml listant les dernières chroniques modifiées
            if (_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateLatestDeezer).Any())
            {
                _deezerOperationsProcessor.PublishLatestReviewsReport(worker, e, xmlReviewsToUpdate, this);
            }

            //Publication du siteMap
            if (_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateSiteMap).Any())
            {
                _deezerOperationsProcessor.PublishSiteMap(worker, e, _miRecordsProvider.GetAllRecords(), this);
            }

            #endregion Deezer

            if (_operationsToPerform.Where(op => op.OperationType == OperationType.GenerateLatestDeezer).Any())
            {
                return (xmlReviewsToUpdate.Count == 0) ? results.Count : xmlReviewsToUpdate.Count;
            }
            return results.Count;
        }

        #region Helpers

        private void ExtractReviewsInfos(BackgroundWorker worker, DoWorkEventArgs e, IList<MIDBRecord> results)
        {
            var count = 0;

            foreach (var result in results)
            {
                try
                {
                    _reviewsManager.AddReviewFrom(result);

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors du parsing de la review {0} : {1}", result.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                Infos = string.Format("Extraction des reviews... {0} sur {1}", ++count, results.Count);
                worker.ReportProgress(count * 100 / (results.Count));
            }
        }

        public static string BuildStandardPath(string fileName)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(path, fileName);
        }

        #endregion

        public ReviewCleaningFormResult ShowReviewCleaningForm(RemovalsPresenter presenter)
        {
            var res = new ReviewCleaningFormResult(presenter.ReviewId);

            var form = new ReviewCleaningForm(ref res, presenter);
            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                return null;
            }

            return res;
        }
    }
}
