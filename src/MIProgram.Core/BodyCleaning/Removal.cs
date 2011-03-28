using FileHelpers;

namespace MIProgram.Core.BodyCleaning
{
    [DelimitedRecord("|")]
    [IgnoreEmptyLines]
    [IgnoreFirst(1)]
    public class Removal
    {
        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.NotAllow)]
        private int _reviewId;
        public int ReviewId
        {
            get { return _reviewId; }
            private set { _reviewId = value; }
        }

        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.AllowForBoth)]
        private string _removedString;
        public string RemovedString
        {
            get { return _removedString; }
            private set { _removedString = value; }
        }

        [FieldQuoted('$', QuoteMode.AlwaysQuoted, MultilineMode.AllowForBoth)]
        private string _replacementString;
        public string ReplacementString
        {
            get { return _replacementString; }
            private set { _replacementString = value; }
        }

        private Removal() { }

        internal Removal(int reviewId, string removedString, string replacementString)
        {
            ReviewId = reviewId;
            RemovedString = removedString;
            ReplacementString = replacementString;
        }
    }
}