using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MIProgram.Core.Logging;
using MIProgram.DataAccess;
using MIProgram.Model;

namespace MIProgram.Core.DataParsers
{
    public static class CountriesRepository
    {
        private static readonly ParsedValueRepository _countryLabelsRepository;
        public static readonly IDictionary<string, string> CountriesLabelsAndCodesDictionnary;

        private static readonly string CountriesLabelsReplacementsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "countriesLabelsReplacements.csv");
        private static readonly string CountriesLabelsAndCodesFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "coutriesList.csv");

        static CountriesRepository()
        {
            CountriesLabelsAndCodesDictionnary = BuildCountriesDictionnary();
            _countryLabelsRepository = new ParsedValueRepository(CountriesLabelsAndCodesDictionnary.Keys.ToList(), CountriesLabelsReplacementsFileName);
        }

        public static ParsedValueRepository Repo
        {
            get { return _countryLabelsRepository; }
        }

        private static IDictionary<string, string> BuildCountriesDictionnary()
        {
            string message = string.Empty;
            var repository = new CSVFileRepository<CountryEntity>(CountriesLabelsAndCodesFileName);
            if (!repository.TestDbAccess(ref message))
            {
                Logging.Logging.Instance.LogError(
                    string.Format("Une erreur est survenue lors du chargement du dictionnaire des pays : {0}", message),
                    ErrorLevel.Error);
            }
            return repository.GetData().ToDictionary(countryEntity => countryEntity.CountryName,
                                                     countryEntity => countryEntity.CountryCode);
        }

        public static IList<Country> ToDomainObject(IList<int> countriesIdxs)
        {
            var result = new List<Country>();

            foreach (var countryIdx in countriesIdxs)
            {
                var countryName = Repo.Values[countryIdx];
                result.Add(new Country(CountriesLabelsAndCodesDictionnary[countryName], countryName));
            }

            return result;
        }
    }
}