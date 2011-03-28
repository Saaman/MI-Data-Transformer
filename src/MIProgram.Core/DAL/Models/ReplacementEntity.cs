using FileHelpers;

namespace MIProgram.DataAccess
{
    [DelimitedRecord(";")]
    [IgnoreEmptyLines]
    public class ReplacementEntity
    {
        private ReplacementEntity() { }

        private string _valueToReplace;
        public string ValueToReplace
        {
            get { return _valueToReplace; }
            private set { _valueToReplace = value; }
        }
        
        private string _replacementValue;
        public string ReplacementValue
        {
            get { return _replacementValue; }
            private set { _replacementValue = value; }
        }

        public ReplacementEntity(string valueToReplace, string replacementValue)
        {
            _valueToReplace = valueToReplace;
            _replacementValue = replacementValue;
        }
    }
}