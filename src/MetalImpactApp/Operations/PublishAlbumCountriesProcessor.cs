using System;
using System.Collections.Generic;
using System.ComponentModel;
using MIProgram.Core;
using MIProgram.Core.DataParsers;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;
using System.Linq;

namespace MetalImpactApp.Operations
{
    public class PublishAlbumCountriesProcessor : IOperationProcessor
    {
        private readonly CountryCodesParser _countryCodesParser;
        private readonly IWriter _writer;
        private readonly string _outputDir;
        private readonly IDictionary<string, CountryDefinition> _countryCodesDefinitions = new Dictionary<string, CountryDefinition>();

        public PublishAlbumCountriesProcessor(IWriter writer, string outputDir)
        {
            _writer = writer;
            _countryCodesParser = new CountryCodesParser();
            _outputDir = outputDir;
        }

        private IDictionary<int, CountryDefinition> InternalProcess(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "extraction des pays d'origine... ";
            var count = 0;
            worker.ReportProgress(count);
            var artistsCountries = new Dictionary<int, CountryDefinition>();

            foreach (var review in managerDeprecated.ParsedReviews)
            {
                try
                {
                    CountryDefinition countryDefinition = null;
                    if (_countryCodesParser.TryParse(review.Album.Artist.OriginCountry, review.Id, ref countryDefinition))
                    {
                        artistsCountries.Add(review.Album.Artist.Id, countryDefinition);
                        if (!_countryCodesDefinitions.Keys.Contains(review.Album.Artist.OriginCountry))
                        {
                            _countryCodesDefinitions.Add(review.Album.Artist.OriginCountry, countryDefinition);
                        }
                    }

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction des pays de l'artiste (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = "extraction des pays d'artistes... ";
                worker.ReportProgress(++count * 100 / (managerDeprecated.ParsedReviews.Count));
            }

            return artistsCountries;
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            InternalProcess(worker, e, managerDeprecated);
            managerDeprecated.Infos = "publication du dictionnaire des pays... ";
            _writer.WriteTextCollection(
                CountryDefinition.CountriesLabelsAndCodesDictionnary.Select(
                    x => string.Format("{0}({1}) : {2} occurences", x.Key, x.Value
                        , _countryCodesDefinitions.Values.Where(y => y.CountryLabels.Contains(x.Key)).Count()))
                .ToList(), "CountriesDictionnary", _outputDir);

            managerDeprecated.Infos = "publication des pays parsés... ";
            _writer.WriteTextCollection(_countryCodesDefinitions.Select(x => string.Format("'{0}' est parsé en '{1}'", x.Key, x.Value.RebuildFromParsedValuesRepository())).ToList(), "Countries", _outputDir);

        }

        public IDictionary<int, CountryDefinition> ProcessMigration(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated, string outputDir)
        {
            var sqlSerializer = new SQLSerializer(_writer, outputDir);

            var artistCountries = InternalProcess(worker, e, managerDeprecated);

            managerDeprecated.Infos = "Ecriture du script SQL...";
            sqlSerializer.SerializeCountries(CountryDefinition.CountriesLabelsAndCodesDictionnary, "countries");

            return artistCountries;
        }
    }
}