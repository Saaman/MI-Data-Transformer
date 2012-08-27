using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileHelpers;

namespace MIProgram.Core.BodyCleaning
{
    public class RemovalManager
    {
        private const string Header = "Review Id|Removed string|Replacement string";

        private readonly string _filePath;
        private readonly IList<Removal> _removals;

        public RemovalManager(string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                throw new FileNotFoundException(string.Format("Containing directory '{0}' does not exist", Path.GetDirectoryName(filePath)));
            }

            _filePath = filePath;

            if (!File.Exists(filePath))
            {
                _removals = new List<Removal>();
                return;
            }

            if (Path.GetExtension(filePath) != ".mi")
            {
                throw new InvalidOperationException(string.Format("{0} file is not of type 'mi'", filePath));
            }

            var engine = new FileHelperEngine(typeof(Removal));
            _removals = engine.ReadFile(filePath).Select(x => x as Removal).ToList();
        }

        public void AddRemoval(Removal removal)
        {
            // try to find a removal with same review Id & same removed string
            var existingRemoval = (from item in _removals
                                   where
                                       (item.ReviewId == removal.ReviewId) &&
                                       (item.RemovedString == removal.RemovedString)
                                   select item).FirstOrDefault();
            // If if does not exists, simply add the new one
            if (existingRemoval == default(Removal))
            {
                _removals.Add(removal);
                return;
            }

            //If the new removal surround an existing one => throw exception
            if(existingRemoval.ReplacementString != removal.ReplacementString)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "A removal already exists to replace '{0}' by '{1}' in review '{2}'. You currently trying to replace it with '{3}' instead.",
                        existingRemoval.RemovedString, existingRemoval.ReplacementString, removal.ReviewId, removal.ReplacementString));
            }

            //Otherwise the same removal is already in the list, no need to do something
        }

        /*public bool Contains(int id, string matchString)
        {
            return (from removal in _removals
                    where (removal.ReviewId == id && removal.RemovedString.ToUpperInvariant() == matchString.ToUpperInvariant())
                    select removal).FirstOrDefault() != null;
        }*/

        public void SaveFile()
        {
            var engine = new FileHelperEngine(typeof(Removal)) { HeaderText = Header };
            engine.WriteFile(_filePath, _removals);
        }

        public IList<Removal> GetRemovalsToApplyOnReview(int reviewId, string reviewText)
        {
            return (from removal in _removals where removal.ReviewId == reviewId select removal).ToList();
        }
    }
}