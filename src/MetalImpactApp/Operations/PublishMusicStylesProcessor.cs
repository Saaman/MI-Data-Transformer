using System;
using System.Collections.Generic;
using System.ComponentModel;
using MIProgram.Core.DataParsers;
using MIProgram.Core.Logging;
using MIProgram.Core.TreeBuilder;
using MIProgram.Core.Writers;
using System.Linq;

namespace MetalImpactApp.Operations
{
    public class PublishMusicStylesProcessor : IOperationProcessor
    {
        private readonly AlbumStylesParser albumStylesParser;
        private readonly IWriter _writer;
        private readonly string _outputDir;

        public PublishMusicStylesProcessor(IWriter writer, string outputDir)
        {
            _writer = writer;
            _outputDir = outputDir;
            albumStylesParser = new AlbumStylesParser();
        }

        internal StylesTree InternalProcess(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "extraction des styles... ";
            var count = 0;
            worker.ReportProgress(count);
            var stylesDefinitions = new Dictionary<string, StyleDefinition>();
            var albumStyles = new Dictionary<int, StyleDefinition>();

            foreach (var review in managerDeprecated.ParsedReviews)
            {
                try
                {
                    StyleDefinition styleDefinition = null;
                    if (albumStylesParser.TryParse(review.Album.MusicType, review.Id, ref styleDefinition))
                    {
                        albumStyles.Add(review.Album.Id, styleDefinition);
                        if (!stylesDefinitions.Keys.Contains(review.Album.MusicType))
                        {
                            stylesDefinitions.Add(review.Album.MusicType, styleDefinition);
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
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction des styles (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = "extraction des styles... ";
                worker.ReportProgress(++count * 100 / (managerDeprecated.ParsedReviews.Count));
            }

            stylesDefinitions = stylesDefinitions.OrderBy(x => x.Value.Complexity).ToDictionary(x => x.Key, y => y.Value);

            var tree = StylesTree.BuildFrom(stylesDefinitions);

            managerDeprecated.Infos = "publication des types de musique... ";
            _writer.WriteTextCollection(
                StyleDefinition.MusicGenresValues.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , stylesDefinitions.Values.Where(y => y.MusicTypes.Contains(x)).Count()))
                .ToList(), "musicTypesDictionnary", _outputDir);

            managerDeprecated.Infos = "publication des styles principaux... ";
            _writer.WriteTextCollection(
                StyleDefinition.MainStylesValues.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , stylesDefinitions.Values.Where(y => y.MainStyles.Contains(x)).Count()))
                .ToList(), "mainStylesDictionnary", _outputDir);

            managerDeprecated.Infos = "publication des associations type de musique / styles principaux... ";
            _writer.WriteTextCollection(StyleDefinition.StylesAssociations.Select(
                    x => string.Format("{0} associé à {1}", x.Key
                        , x.Value)).ToList(),
                        "musicStylesAndTypesAssociationsDictionnary", _outputDir);

            managerDeprecated.Infos = "publication des altérations de style... ";
            _writer.WriteTextCollection(StyleDefinition.StyleAlterationsValues.Select(
                    x => string.Format("{0} : {1} occurences", x
                        , stylesDefinitions.Values.Where(y => y.StyleAlterations.Contains(x)).Count())
                ).ToList(), "StyleAlterationsDictionnary", _outputDir);


            managerDeprecated.Infos = "publication des styles parsés... ";
            _writer.WriteTextCollection(stylesDefinitions.Select(x => string.Format("'{0}' est parsé en '{1}'", x.Key, x.Value.RebuildFromParsedValuesRepository())).ToList(), "Styles", _outputDir);

            return tree;
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            InternalProcess(worker, e, managerDeprecated);
        }
    }
}
