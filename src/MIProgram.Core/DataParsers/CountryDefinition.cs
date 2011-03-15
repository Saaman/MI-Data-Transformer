using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MIProgram.Core.Logging;
using MIProgram.DataAccess;

namespace MIProgram.Core.DataParsers
{
    public class CountryDefinition : IFieldDefinition
    {
        private static readonly string CountriesLabelsReplacementsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "countriesLabelsReplacements.csv");
        private static readonly string CountriesLabelsAndCodesFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "coutriesList.csv");

        public IList<string> CountryLabels
        {
            get { return _countryLabelsIdxs.Select(x => CountryLabelsRepository.Values[x]).ToList(); }
        }
        public static readonly IDictionary<string, string> CountriesLabelsAndCodesDictionnary;
        internal static readonly ParsedValueRepository CountryLabelsRepository;

        private static IDictionary<string, string> BuildCountriesDictionnary()
        {
            string message = string.Empty;
            var repository = new CSVFileRepository<CountryEntity>(CountriesLabelsAndCodesFileName);
            if (!repository.TestDbAccess(ref message))
            {
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors du chargement du dictionnaire des pays : {0}", message), ErrorLevel.Error);
            }
            return repository.GetData().ToDictionary(countryEntity => countryEntity.CountryName, countryEntity => countryEntity.CountryCode);
        }

        private readonly IList<int> _countryLabelsIdxs;

        public CountryDefinition(IList<int> countryLabelsIdxs)
        {
            _countryLabelsIdxs = countryLabelsIdxs;
        }

        static CountryDefinition()
        {
            CountriesLabelsAndCodesDictionnary = BuildCountriesDictionnary();
            CountryLabelsRepository = new ParsedValueRepository(CountriesLabelsAndCodesDictionnary.Keys.ToList(), CountriesLabelsReplacementsFileName);
        }

        public string RebuildFromParsedValuesRepository()
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
        }
    }
}