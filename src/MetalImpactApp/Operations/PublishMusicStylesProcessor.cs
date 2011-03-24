using System;
using MIProgram.Core;
using MIProgram.Core.DataParsers;
using MIProgram.Core.ProductStores;
using MIProgram.Core.Writers;
using System.Linq;
using MIProgram.Model;

namespace MetalImpactApp.Operations
{
    public class PublishMusicStylesProcessor : IOperationProcessor<Album>
    {
        private readonly IWriter _writer;

        public PublishMusicStylesProcessor(IWriter writer)
        {
            _writer = writer;
        }

        public void Process(ProductRepository<Album> productRepository)
        {
            var albumRepository = productRepository as AlbumRepository;
            var outputDir = Constants.FieldsExtractionsOutputDirectoryPath;

            if (albumRepository == null)
            {
                throw new InvalidCastException("ProductRepository cannot be cast to AlbumRepository");
            }

            _writer.WriteTextCollection(
                StylesRepository.MusicGenresRepository.Values.Select(
                    x => string.Format("{0} : {1} occurences", x,
                                       albumRepository.ExplodedReviews.Where(y => y.ProcessedAlbumStyle.MusicTypes.Contains(x)).Count()))
                    .ToList(), "musicTypesDictionnary", outputDir);

            _writer.WriteTextCollection(
                StylesRepository.MainStylesRepository.Values.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , albumRepository.ExplodedReviews.Where(y => y.ProcessedAlbumStyle.MainStyles.Contains(x)).Count()))
                .ToList(), "mainStylesDictionnary", outputDir);

            _writer.WriteTextCollection(StylesRepository.StylesAssociations.Select(
                    x => string.Format("{0} associé à {1}", StylesRepository.MainStylesRepository.Values[x.Key]
                        , StylesRepository.MusicGenresRepository.Values[x.Value])).ToList(),
                        "musicStylesAndTypesAssociationsDictionnary", outputDir);

            _writer.WriteTextCollection(StylesRepository.StyleAlterationsRepository.Values.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , albumRepository.ExplodedReviews.Where(y => y.ProcessedAlbumStyle.StyleAlterations.Contains(x)).Count())
                ).ToList(), "StyleAlterationsDictionnary", outputDir);

            _writer.WriteTextCollection(albumRepository.ExplodedReviews.Select(x => string.Format("'{0}' est parsé en '{1}'", x.AlbumMusicGenre, x.ProcessedAlbumStyle.RebuildFromParsedValuesRepository())).ToList(), "Styles", outputDir);

        }

        public string ProcessDescription
        {
            get { return "publication des types de musique... "; }
        }
    }
}
