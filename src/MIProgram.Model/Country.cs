using MIProgram.Model.Extensions;

namespace MIProgram.Model
{
    public class Country
    {
        public string CountryCode { get; private set; }
        public string CountryName { get; private set; }

        public Country(string countryCode, string countryName)
        {
            CountryCode = countryCode.ToUpperInvariant();
            CountryName = countryName.ToCamelCase();
        }

        #region IEquality members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Country)) return false;
            return Equals((Country) obj);
        }

        public bool Equals(Country obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.CountryName, CountryName);
        }

        public override int GetHashCode()
        {
            return (CountryName != null ? CountryName.GetHashCode() : 0);
        }

        #endregion
    }
}