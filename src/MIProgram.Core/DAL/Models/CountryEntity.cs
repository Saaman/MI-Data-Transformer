using FileHelpers;

namespace MIProgram.Core.DAL.Models
{
    [DelimitedRecord(";")]
    [IgnoreEmptyLines]
    public class CountryEntity
    {
        private CountryEntity() { }

        private string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            private set { _countryName = value; }
        }
        
        private string _countryCode;
        public string CountryCode
        {
            get { return _countryCode; }
            private set { _countryCode = value; }
        }
        
        public CountryEntity(string countryName, string countryCode)
        {
            _countryName = countryName;
            _countryCode = countryCode;
        }
    }
}