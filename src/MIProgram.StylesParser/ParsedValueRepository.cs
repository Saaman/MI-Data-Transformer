using System.Collections.Generic;

namespace MIProgram.StylesParser
{
    public class ParsedValueRepository
    {
        public IList<string> Values { get; private set; }

        protected internal ParsedValueRepository()
        {
            Values = new List<string>();
        }

        public int? AddOrRetrieveValueIndex(string value)
        {
            var newValue = value.Trim().ToUpperInvariant();
            if(string.IsNullOrEmpty(newValue))
            {
                return null;
            }
            if(!Values.Contains(newValue))
            {
                Values.Add(newValue);
            }

            return Values.IndexOf(newValue);
        }
    }
}
