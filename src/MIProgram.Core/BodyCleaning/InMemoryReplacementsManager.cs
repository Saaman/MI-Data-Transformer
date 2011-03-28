using System.Collections.Generic;

namespace MIProgram.Core.BodyCleaning
{
    public class InMemoryReplacementsManager
    {
        private readonly IDictionary<string, string> _replacements = new Dictionary<string, string>();

        public IDictionary<string, string> Replacements { get { return _replacements; } }

        /*public string ApplyReplacementsOn(int reviewId, string text, RemovalManager main)
        {
            string result = text;
            var removals = (from removal in _replacements where text.Contains(removal.RemovedString) select removal).ToList();

            foreach (var removal in removals)
            {
                if (result.Contains(removal.RemovedString))
                {
                    result = result.Replace(removal.RemovedString, removal.ReplacementString);
                    main.AddRemoval(reviewId, removal.RemovedString, removal.ReplacementString);
                    Logging.Logging.Instance.LogError(string.Format("Le texte {0} a été remplacé par {2} de la review {1}", removal.RemovedString, reviewId, removal.ReplacementString), ErrorLevel.Info);
                }
                else
                {
                    _replacements.Remove(removal);
                    Logging.Logging.Instance.LogError(string.Format("Le texte {0} ne peut plus être trouvé dans la review {1} : il ne sera pas plus examiné au prochain run", removal, reviewId), ErrorLevel.Info);
                }
            }

            return result;
        }*/

        /*public void AddRemoval(int reviewId, string removedString, string replacementString)
        {
            _replacements.Add(new Removal(reviewId, removedString, replacementString));
        }

        public void AddRemoval(int reviewId, string removedString)
        {
            AddRemoval(reviewId, removedString, string.Empty);
        }

        public void AddRemoval(Removal removal)
        {
            _replacements.Add(removal);
        }*/

        /*public bool Contains(int id, string matchString)
        {
            return (from removal in _replacements 
                    where (removal.ReviewId == id && removal.RemovedString.ToUpperInvariant() == matchString.ToUpperInvariant()) 
                    select removal).FirstOrDefault() != null;
        }*/

        public void AddReplacement(string removedString, string replacementString)
        {
            _replacements.Add(removedString, replacementString);
        }
    }
}