using System;
using System.Collections.Generic;
using MetalImpactApp.OperationManagement.AlbumImpl;
using MIProgram.Core.AlbumImpl;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Model;
using MIProgram.Core.Writers;

namespace MetalImpactApp.OperationManagement
{
    public class ReviewProcessorBuilder
    {
        private static OperationManager<Album> BuildAlbumReviewProcessor(string sourceFile, IWriter outputsWriter, List<Func<Product, bool>> filtersDefinitions, IList<Operation> operations, DateTime lastExportDate)
        {
            IMIRecordsProvider provider;
            CSVFileProvider.TryBuildProvider(sourceFile, out provider);
            var reviewExploder = new AlbumReviewExploder();
            var reviewProcessor = new AlbumReviewProcessor(provider, reviewExploder, new CleaningFormLauncher());
            return new AlbumOperationManager(reviewProcessor, operations, outputsWriter, filtersDefinitions, lastExportDate);
        }

        public IOperationsLauncher BuildFor<T>(string sourceFile, IWriter outputsWriter, List<Func<Product, bool>> filtersDefinitions, IList<Operation> operations, DateTime lastExportDate)
        {
            if (typeof(T) == typeof(Album))
            {
                return BuildAlbumReviewProcessor(sourceFile, outputsWriter, filtersDefinitions, operations, lastExportDate);
            }
            throw new InvalidOperationException(string.Format("No builder is defined to manage review of type '{0}'", typeof(T)));
        }
    }
}