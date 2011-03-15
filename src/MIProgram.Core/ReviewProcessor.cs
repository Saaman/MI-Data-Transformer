using System.Collections.Generic;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.ProductStores;
using MIProgram.Model;

namespace MIProgram.Core
{
    public abstract class ReviewProcessor<T> where T : Product
    {
        private readonly IMIRecordsProvider _miRecordsProvider;
        private readonly IReviewExploder<T> _reviewExploder;
        protected IProductRepository<T> ProductRepository;

        protected ReviewProcessor(IMIRecordsProvider miRecordsProvider, IReviewExploder<T> reviewExploder)
        {
            _miRecordsProvider = miRecordsProvider;
            _reviewExploder = reviewExploder;
        }

        protected void Process()
        {
            IList<IExplodedReview<T>> explodedReviews = new List<IExplodedReview<T>>();

            //get reviews
            var reviews = _miRecordsProvider.GetRecords();

            // integrate each review
            foreach (var review in reviews)
            {
                explodedReviews.Add(_reviewExploder.ExplodeReviewFrom(review));
            }

            // post process
        }
    }
}