using System.Collections.Generic;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;

namespace MIProgram.Core
{
    public abstract class ReviewProcessor<T> where T: Product
    {
        private readonly IMIRecordsProvider _miRecordsProvider;
        protected readonly IReviewExploder<T> ReviewExploder;
        private readonly ProductReviewBodyCleaner<T> _reviewBodyCleaner;
        private readonly bool _doReviewCleaning;

        protected ReviewProcessor(IMIRecordsProvider miRecordsProvider, IReviewExploder<T> reviewExploder, ProductReviewBodyCleaner<T> reviewBodyCleaner, bool doReviewCleaning)
        {
            _miRecordsProvider = miRecordsProvider;
            _reviewBodyCleaner = reviewBodyCleaner;
            _doReviewCleaning = doReviewCleaning;
            ReviewExploder = reviewExploder;
        }

        public void Process(AsyncWorkerWrapper asyncWorkerWrapper, ProductRepository<T> productRepository)
        {
            IList<IExplodedReview<T>> explodedReviews = new List<IExplodedReview<T>>();

            var count = 0;
            asyncWorkerWrapper.BackgroudWorker.ReportProgress(count);
            asyncWorkerWrapper.Infos = "Récupération des reviews";

            //get reviews
            var reviews = _miRecordsProvider.GetAllRecords();

            foreach (var review in reviews)
            {
                // explode each review
                var explodedReview = ReviewExploder.ExplodeReviewFrom(review);
                
                //clean review text
                if (_doReviewCleaning)
                {
                    explodedReview.CleanTextUsing(_reviewBodyCleaner.CleanReviewBody);
                }

                //Integrate review
                explodedReviews.Add(explodedReview);

                //Perform additional unit work
                SpecificProcess(explodedReview);

                if (asyncWorkerWrapper.BackgroudWorker.CancellationPending)
                {
                    asyncWorkerWrapper.WorkEventArgs.Cancel = true;
                    break;
                }

                asyncWorkerWrapper.BackgroudWorker.ReportProgress(++count * 100 / reviews.Count);
            }

            // post process
            PostProcessExplodedReviews(explodedReviews);

            //Integrate reviews in Product repository
            foreach (var explodedReview in explodedReviews)
            {
                productRepository.Add(explodedReview);
            }

            // post process
            PostProcessProductRepository(productRepository);

            //Finalize work
            FinalizeWork();
        }

        public void FinalizeWork()
        {
            if (_reviewBodyCleaner != null)
                _reviewBodyCleaner.FinalizeWork();
        }

        protected abstract void PostProcessExplodedReviews(IList<IExplodedReview<T>> explodedReviews);
        protected abstract void SpecificProcess(IExplodedReview<T> explodedReview);
        protected abstract void PostProcessProductRepository(ProductRepository<T> productRepository);
    }
}