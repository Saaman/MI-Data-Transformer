using MIProgram.Core.Logging;

namespace MIProgram.Core.Cleaners
{
    public class ReviewTextCleaner
    {
        private readonly RemovalManager _removalManager = new RemovalManager(Constants.MIRemovalsFilePath);
        private readonly InMemoryReplacementsManager _inMemoryReplacementsManager = new InMemoryReplacementsManager();

        public string CleanText(int reviewId, string text)
        {
            string result = text;
            var removalsToApply = _removalManager.GetRemovalsToApplyOnReview(reviewId, text);

            foreach (var removal in removalsToApply)
            {
                if (result.Contains(removal.RemovedString))
                {
                    result = result.Replace(removal.RemovedString, removal.ReplacementString);
                    Logging.Logging.Instance.LogError(string.Format("Apply Removals : Le texte {0} a été remplacé par {1} de la review {2}", removal.RemovedString, removal.ReplacementString, reviewId), ErrorLevel.Info);
                }
                else
                {
                    //_removals.Remove(removal);
                    Logging.Logging.Instance.LogError(string.Format("Apply Removals : Le texte {0} ne peut plus être trouvé dans la review {1}", removal.RemovedString, reviewId), ErrorLevel.Info);
                }
            }

            foreach (var replacement in _inMemoryReplacementsManager.Replacements)
            {
                if (result.Contains(replacement.Key))
                {
                    result = result.Replace(replacement.Key, replacement.Value);
                    _removalManager.AddRemoval(new Removal(reviewId, replacement.Key, replacement.Value));
                    Logging.Logging.Instance.LogError(string.Format("Apply Replacement : Le texte {0} a été remplacé par {1} de la review {2}", replacement.Key, replacement.Value, reviewId), ErrorLevel.Info);
                }
            }

            return result;
        }

        public string AddAndApplyRemoval(Removal removal, bool repeatOnOtherReviews, string text)
        {
            var result = text;

            _removalManager.AddRemoval(removal);
            if (repeatOnOtherReviews)
            {
                _inMemoryReplacementsManager.AddReplacement(removal.RemovedString, removal.ReplacementString);
            }

            if (result.Contains(removal.RemovedString))
            {
                result = result.Replace(removal.RemovedString, removal.ReplacementString);
                Logging.Logging.Instance.LogError(string.Format("Add an apply Removals : Le texte {0} a été remplacé par {1} de la review {2}", removal.RemovedString, removal.ReplacementString, removal.ReviewId), ErrorLevel.Info);
            }

            return result;
        }

        public void FinalizeWork()
        {
            if (_removalManager != null)
                _removalManager.SaveFile();
        }
    }
}