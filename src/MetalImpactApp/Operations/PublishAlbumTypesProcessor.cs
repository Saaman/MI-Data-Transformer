using System;
using System.Collections.Generic;
using System.ComponentModel;
using MIProgram.Core.DataParsers;
using MIProgram.Core.Logging;
using MIProgram.Core.Writers;
using System.Linq;

namespace MetalImpactApp.Operations
{
    public class PublishAlbumTypesProcessor : IOperationProcessor
    {
        private readonly AlbumTypesParser _albumTypesParser;
        private readonly IWriter _writer;
        private readonly string _outputDir;

        public PublishAlbumTypesProcessor(IWriter writer, string outputDir)
        {
            _writer = writer;
            _outputDir = outputDir;
            _albumTypesParser = new AlbumTypesParser();
        }

        internal IDictionary<int, AlbumTypeDefinition> InternalProcess(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            managerDeprecated.Infos = "extraction des types d'albums... ";
            var count = 0;
            worker.ReportProgress(count);
            var albumTypesDefinitions = new Dictionary<string, AlbumTypeDefinition>();
            var albumTypes = new Dictionary<int, AlbumTypeDefinition>();

            foreach (var review in managerDeprecated.ParsedReviews)
            {
                try
                {
                    AlbumTypeDefinition albumTypeDefinition = null;
                    if (_albumTypesParser.TryParse(review.Album.RecordType, review.Id, ref albumTypeDefinition))
                    {
                        albumTypes.Add(review.Album.Id, albumTypeDefinition);
                        if (!albumTypesDefinitions.Keys.Contains(review.Album.RecordType))
                        {
                            albumTypesDefinitions.Add(review.Album.RecordType, albumTypeDefinition);
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
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction des types d'album (review {0}) : {1}", review.Id, ex.Message), ErrorLevel.Error);
                    continue;
                }

                managerDeprecated.Infos = "extraction des types d'albums... ";
                worker.ReportProgress(++count * 100 / (managerDeprecated.ParsedReviews.Count));
            }

            managerDeprecated.Infos = "publication des types d'album parsés... ";
            _writer.WriteTextCollection(albumTypesDefinitions.Select(x => string.Format("'{0}' est parsé en '{1}'", x.Key, x.Value.RebuildFromParsedValuesRepository())).ToList(), "AlbumTypes", _outputDir);

            return albumTypes;
        }

        public void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated)
        {
            InternalProcess(worker, e, managerDeprecated);
        }
    }
}
