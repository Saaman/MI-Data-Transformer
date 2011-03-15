using System;
using System.Collections.Generic;

namespace MIProgram.Core
{
    public class RemovalsPresenter
    {
        public int ReviewId { get; private set; }
        public string Title { get; private set; }
        public string ReviewerName { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string ReviewText { get; private set; }

        public SortedList<int, string> MatchedStrings { get; private set; }

        public RemovalsPresenter(int reviewId, string title, string reviewerName, DateTime createdDate, string reviewText)
        {
            ReviewId = reviewId;
            Title = title;
            ReviewerName = reviewerName;
            CreatedDate = createdDate;
            ReviewText = reviewText;

            MatchedStrings = new SortedList<int, string>();
        }

        public void AddMatchedString(string matchedString, int index)
        {
            if(string.IsNullOrEmpty(matchedString))
            {
                return;
            }
            MatchedStrings.Add(index, matchedString);
        }
    }
}
