using System.Collections.Generic;
using System.Linq;
using MIProgram.Model;

namespace MIProgram.Core.DataParsers
{
    public class CountryDefinition : IToDomainObject<Country>
    {
        public IList<string> CountryLabels
        {
            get { return _countryLabelsIdxs.Select(x => CountriesRepository.Repo.Values[x]).ToList(); }
        }

        private readonly IList<int> _countryLabelsIdxs;

        public CountryDefinition(IList<int> countryLabelsIdxs)
        {
            _countryLabelsIdxs = countryLabelsIdxs;
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
        public Country ToDomainObject()
        {
            throw new System.NotImplementedException();
        }
    }
}