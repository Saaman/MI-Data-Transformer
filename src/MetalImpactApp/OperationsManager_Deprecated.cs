using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using MetalImpactApp.Operations;
using MIProgram.Core;
using MIProgram.Core.AlbumImpl;
using MIProgram.Core.Cleaners;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Writers;
using MIProgram.DataAccess;
using MIProgram.Model;

namespace MetalImpactApp
{
    public class OperationsManager_Deprecated : ICanShowReviewCleaningForm
    {
        public bool IsWorking { get; set; }
        public string Infos { get; set; }

        private readonly IMIRecordsProvider _miRecordsProvider;
        private readonly ReviewsManager _reviewsManager;
        private readonly DateTime _lastUpdateDate;
        private readonly IList<Operation> _operationsToPerform;
        
        private readonly IDictionary<OperationType, IOperationProcessor> _operationsProcessorsDic;
        private readonly ParseReviewsProcessor _reviewsExtractorProcessor;

        public OperationsManager_Deprecated(IMIRecordsProvider miRecordsProvider, IWriter writer, DateTime lastUpdateDate, IList<Operation> operationsToPerform)
        {
            _miRecordsProvider = miRecordsProvider;
            _lastUpdateDate = lastUpdateDate;
            _operationsToPerform = operationsToPerform;

            var mainCleaner = RemovalManager.GetReviewCleanerFromFile(Constants.MIRemovalsFilePath);
            var reviewBuilder = new AlbumReviewBodyCleaner(mainCleaner, new InMemoryReplacementsManager(), this);
            _reviewsManager = new ReviewsManager(reviewBuilder);

            _operationsProcessorsDic = new Dictionary<OperationType, IOperationProcessor>
                {
                {OperationType.PublishMusicStyles, new PublishMusicStylesProcessor(writer, Constants.FieldsExtractionsOutputDirectoryPath)},
                {OperationType.PublishAlbumTypes, new PublishAlbumTypesProcessor(writer, Constants.FieldsExtractionsOutputDirectoryPath)},
                {OperationType.PublishAlbumCountries, new PublishAlbumCountriesProcessor(writer, Constants.FieldsExtractionsOutputDirectoryPath)},
                {OperationType.PublishReviewsWithNewModel, new PublishReviewsWithNewModelProcessor(writer, Constants.MigrationOperationsOutputDirectoryPath)},
                {OperationType.PublishReviewsXMLsForDeezer, new PublishReviewsXMLsForDeezerProcessor(writer, Constants.DeezerOperationsOutputDirectoryPath)},
                {OperationType.PublishFullDeezerListing, new PublishFullDeezerListingProcessor(writer, Constants.DeezerOperationsOutputDirectoryPath)},
                {OperationType.PublishDiffDeezerListing, new PublishDiffDeezerListingProcessor(writer, Constants.DeezerOperationsOutputDirectoryPath)},
                {OperationType.PublishSiteMap, new PublishSiteMapProcessor(writer, Constants.DeezerOperationsOutputDirectoryPath)}
                };

            _reviewsExtractorProcessor = new ParseReviewsProcessor();

            IsWorking = false;
        }

        public IList<Review> ParsedReviews { get { return _reviewsManager.Reviews; } }
        public IList<Reviewer> ParsedReviewers { get { return _reviewsManager.Reviewers; } }
        public IList<Artist> ParsedArtists { get { return _reviewsManager.Artists; } }
        public IList<Review> ParsedReviewsToUpdate { get { return _reviewsManager.Reviews.Where(x => x.LastUpdateDate > _lastUpdateDate).ToList(); } }
        public IList<MIDBRecord> AllOriginalReviews { get { return _miRecordsProvider.GetAllRecords(); } }
        public IList<MIDBRecord> FilteredOriginalReviews { get { return _miRecordsProvider.GetRecords(); } }

        public void EndWork()
        {
            IsWorking = false;
            _reviewsManager.FinalizeWork();
        }

        public long Process(BackgroundWorker worker, DoWorkEventArgs e)
        {
            IsWorking = true;

            _reviewsExtractorProcessor.Process(worker, e, this);

            foreach (var operation in _operationsToPerform)
            {
                _operationsProcessorsDic[operation.OperationType].Process(worker, e, this);
            }

            return ParsedReviews.Count;
        }

        public void AddReviewFrom(MIDBRecord midbRecord)
        {
            _reviewsManager.AddReviewFrom(midbRecord);
        }

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
