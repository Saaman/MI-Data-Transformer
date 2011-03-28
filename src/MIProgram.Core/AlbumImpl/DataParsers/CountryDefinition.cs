using System.Collections.Generic;
using MIProgram.Model;

namespace MIProgram.Core.DataParsers
{
    public class CountryDefinition : IToDomainObject<IList<Country>>
    {
        /*public IList<string> CountryLabels
        {
            get { return _countryIdxs.Select(x => CountriesRepository.Repo.Values[x]).ToList(); }
        }*/

        private readonly IList<int> _countryIdxs;

        public CountryDefinition(IList<int> countryIdxs)
        {
            _countryIdxs = countryIdxs;
        }

        /*public string RebuildFromParsedValuesRepository()
        {
            var result = string.Empty;
            foreach (var countryLabel in CountryLabels)
            {
                if (CountriesLabelsAndCodesDictionnary.Keys.Contains(countryLabel))
                {
                    result += string.Format("{0}({1})", countryLabel, CountriesLabelsAndCodesDictionnary[countryLabel]);
                }
                else
                {
                    result += countryLabel;
                }
                result += "-";
            }
            return result.Trim('-');
        }*/
        public IList<Country> ToDomainObject()
        {
            var result = new List<Country>();
            foreach (var _countryIdx in _countryIdxs)
            {
                result.Add(new Country());
            }
        }
    }
}