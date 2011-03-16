using System;
using System.Linq;
using MIProgram.Core;
using MIProgram.Core.DataParsers;
using MIProgram.Core.ProductStores;
using MIProgram.Core.Writers;
using MIProgram.Model;

namespace MetalImpactApp
{
    public class PublishArtistCountriesOperation : IOperationProcessor<Album>
    {
        private readonly IWriter _writer;

        public PublishArtistCountriesOperation(IWriter _writer)
        {
            this._writer = _writer;
        }

        public void Process(IProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;
            var outputDir = Constants.FieldsExtractionsOutputDirectoryPath;

            if(albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }


            var countryCodesDefinitions = albumRepository.CountryCodesDefinitions;
            _writer.WriteTextCollection(
                CountryDefinition.CountriesLabelsAndCodesDictionnary.Select(
                    x => string.Format("{0}({1}) : {2} occurences", x.Key, x.Value
                                       , countryCodesDefinitions.Values.Where(y => y.CountryLabels.Contains(x.Key)).Count()))
                    .ToList(), "CountriesDictionnary", outputDir);

            _writer.WriteTextCollection(countryCodesDefinitions.Select(x => string.Format("'{0}' est parsé en '{1}'", x.Key, x.Value.RebuildFromParsedValuesRepository())).ToList(), "Countries", outputDir);

        }

        public string ProcessDescription
        {
            get { return "Publication des pays parsés... "; }
        }
    }
}