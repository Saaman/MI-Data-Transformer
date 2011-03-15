using System.Collections.Generic;
using System.Text.RegularExpressions;
using MIProgram.Core.Cleaners;
using MIProgram.Model;

namespace MIProgram.Core
{
    public abstract class ProductReviewBodyCleaner<T> where T: Product
    {
        private readonly ICanShowReviewCleaningForm _form;
        protected ReviewTextCleaner _reviewTextCleaner = new ReviewTextCleaner();
        protected abstract IList<string> TextCleaningPatterns { get; }

        protected ProductReviewBodyCleaner(ICanShowReviewCleaningForm form)
        {
            _form = form;
        }

        public string CleanReviewBody(IExplodedReview<T> explodedReview)
        {
            var result = _reviewTextCleaner.CleanText(explodedReview.RecordId, explodedReview.ReviewBody);

            var removalsPresenter = new RemovalsPresenter(explodedReview.RecordId, explodedReview.RecordTitle, explodedReview.ReviewerName, explodedReview.RecordCreationDate, result);

            // Rechercher les patterns
            foreach (var pattern in TextCleaningPatterns)
            {
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(result);

                foreach (Match match in matches)
                {                                      
                    var matchString = match.Groups[1].ToString();
                    /*//Test to avoid to repeat DoNothing actions
                    if(!_reviewTextCleaner.Contains(record.Id, matchString))
                    {*/
                        if (!removalsPresenter.MatchedStrings.ContainsKey(result.IndexOf(matchString)))
                        {
                            removalsPresenter.AddMatchedString(matchString, result.IndexOf(matchString));
                        }
                    //}
                }
            }

            if (removalsPresenter.MatchedStrings.Count == 0)
            {
                return result;
            }

            var newRemovals = _form.ShowReviewCleaningForm(removalsPresenter);
            foreach (KeyValuePair<Removal, bool> newR in newRemovals.Removals)
            {
                result = _reviewTextCleaner.AddAndApplyRemoval(newR.Key, newR.Value, result);
            }

            //clean de fin
            /*result = _reviewTextCleaner.CleanText(record.Id, result);
            result = _temporaryReplacementsManager.ApplyReplacementsOn(record.Id, result, _reviewTextCleaner);*/
            return result;
        }
    }
}
