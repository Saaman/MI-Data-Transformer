using System.Collections.Generic;
using MIProgram.Core.BodyCleaning;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;

namespace MIProgram.Core
{
    public abstract class ReviewProcessor<T> where T: Product
    {
        private readonly IMIRecordsProvider _miRecordsProvider;
        protected readonly IReviewExploder<T> _reviewExploder;
        private readonly ReviewTextCleaner _reviewTextCleaner = new ReviewTextCleaner();

        protected ReviewProcessor(IMIRecordsProvider miRecordsProvider, IReviewExploder<T> reviewExploder)
        {
            _miRecordsProvider = miRecordsProvider;
            _reviewExploder = reviewExploder;
        }

        public void Process(AsyncWorkerWrapper asyncWorkerWrapper, ProductRepository<T> productRepository)
        {
            IList<IExplodedReview<T>> explodedReviews = new List<IExplodedReview<T>>();

            //get reviews
            var reviews = _miRecordsProvider.GetAllRecords();

            foreach (var review in reviews)
            {
                // explode each review
                var explodedReview = _reviewExploder.ExplodeReviewFrom(review);
                
                //clean review text
                explodedReview.CleanTextUsing(_reviewTextCleaner.CleanText);

                //Integrate review
                explodedReviews.Add(explodedReview);

                //Perform additional unit work
                SpecificProcess(explodedReview);
            }

            // post process
            PostProcess(explodedReviews);

            //Integrate reviews in Product repository
            foreach (var explodedReview in explodedReviews)
            {
                productRepository.Add(explodedReview);
            }

            //Attach additional repositories & processed stuff to ProductRepoistory
            FinalizeProductRepository(productRepository);

            //Finalize work
            FinalizeWork();
        }

        public void FinalizeWork()
        {
            if (_reviewTextCleaner != null)
                _reviewTextCleaner.FinalizeWork();
        }

        protected abstract void PostProcess(IList<IExplodedReview<T>> explodedReviews);
        protected abstract void SpecificProcess(IExplodedReview<T> explodedReview);
        protected abstract void FinalizeProductRepository(ProductRepository<T> productRepository);
    }
}