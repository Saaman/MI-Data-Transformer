using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core;
using MIProgram.Core.AlbumImpl.LocalRepositories;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;
using MIProgram.Core.Writers;
using MIProgram.Core.Extensions;

namespace MetalImpactApp.OperationManagement.AlbumImpl.Operations
{
    public class PublishArtistCountriesProcessor : IOperationProcessor<Album>
    {
        private readonly IWriter _writer;

        public PublishArtistCountriesProcessor(IWriter _writer)
        {
            this._writer = _writer;
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;
            var outputDir = Constants.FieldsExtractionsOutputDirectoryPath;

            if(albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            _writer.WriteTextCollection(
                CountriesRepository.CountriesLabelsAndCodesDictionnary.Select(
                    x => string.Format("{0}({1}) : {2} occurences", x.Key, x.Value
                                       , productRepository.Artists.Where(y => y.Countries.Select(co => co.CountryName).Contains(x.Key, new UpperInvariantComparer())).Count()))
                    .ToList(), "CountriesDictionnary", outputDir);

            _writer.WriteTextCollection(albumRepository.ExplodedReviews.Select(x => string.Format("'{0}' est parsé en '{1}'", x.ArtistCountry, TextSerialize(x.ProcessedArtistCountries))).ToList(), "Countries", outputDir);

        }

        private static string TextSerialize(IEnumerable<Country> countries)
        {
            var result = string.Empty;
            result = countries.Aggregate(result, (seed, country) => seed + country.CountryName + "(" + country.CountryCode + ") / ", x => x.TrimEnd(new[] {' ', '/'}));
            return result;
        }

        public string ProcessDescription
        {
            get { return "Publication des pays parsés... "; }
        }
    }
}