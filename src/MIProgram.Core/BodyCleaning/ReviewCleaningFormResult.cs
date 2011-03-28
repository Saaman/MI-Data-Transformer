using System.Collections.Generic;
using System.Linq;

namespace MIProgram.Core.BodyCleaning
{
    public class ReviewCleaningFormResult
    {
        public int ReviewId { get; private set; }
        public IDictionary<Removal, bool> Removals {get; private set;}

        public ReviewCleaningFormResult(int reviewId)
        {
            ReviewId = reviewId;
            Removals = new Dictionary<Removal, bool>();
        }

        public Removal AddRemoval(int reviewId, string oldText, string newText, bool repeatInAllReviews)
        {
            var existingRemoval = (from removal in Removals.Keys where (removal.RemovedString == oldText && removal.ReviewId == reviewId) select removal).FirstOrDefault();

            if(existingRemoval != null)
            {
                return existingRemoval;
            }

            var newRemoval = new Removal(reviewId, oldText, newText);
            Removals.Add(newRemoval, repeatInAllReviews);

            return null;
        }

        public void CleanRemovals()
        {
            Removals = new Dictionary<Removal, bool>();
        }
    }
}