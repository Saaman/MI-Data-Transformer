using System.Collections.Generic;
using System.Text.RegularExpressions;
using MIProgram.DataAccess;
using MIProgram.Core.Cleaners;

namespace MIProgram.Core
{
    partial class ReviewBuilder_Deprecated
    {
        private readonly IList<string> _textCleaningPatterns = new List<string> {
                                                                @"(<\s*a\s+[^>]+>([^<>]+)<\s*/a\s*>)",
                                                                @"(<\s*u\s*>([^<>]+)<\s*/u\s*>)",
                                                                @"(<\s*img\s*>([^<>]+)<\s*/img\s*>)",
                                                                @"(<\s*[^<>]\s*>\s*(discographie|tracklisting|distribution)[^<>]+<\s*/[^<>]\s*>)" };
        
        private string ExtractText(MIDBRecord record)
        {
            //var result = ExtractInfo(_textRegex, record.Text, record.Id, "Text");
            
            //clean préalable
            /*result = _reviewTextCleaner.CleanText(record.Id, result);
            result = _temporaryReplacementsManager.ApplyReplacementsOn(record.Id, result, _reviewTextCleaner);
            */
            result = _reviewTextCleaner.CleanText(record.Id, result);

            var removalsPresenter = new RemovalsPresenter(record.Id, record.Title, record.ReviewerName, record.CreationDate, result);

            // Rechercher les patterns
            foreach (var pattern in _textCleaningPatterns)
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

            var newRemovals = _mainForm.ShowReviewCleaningForm(removalsPresenter);
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
