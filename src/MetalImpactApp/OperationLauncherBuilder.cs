using System;
using System.Collections.Generic;
using MIProgram.Core;
using MIProgram.Core.AlbumImpl;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Writers;
using MIProgram.Model;

namespace MetalImpactApp
{
    public class ReviewProcessorBuilder
    {
        private static OperationManager<Album> BuildAlbumReviewProcessor(string sourceFile, IWriter outputsWriter, List<Func<Product, bool>> filtersDefinitions, IList<Operation> operations)
        {
            IMIRecordsProvider provider;
            CSVFileProvider.TryBuildProvider(sourceFile, out provider);
            var reviewExploder = new AlbumReviewExploder();
            var reviewProcessor = new AlbumReviewProcessor(provider, reviewExploder);
            return new AlbumOperationManager(reviewProcessor, operations, outputsWriter, filtersDefinitions);
        }

        public IOperationsLauncher BuildFor<T>(string sourceFile, IWriter outputsWriter, List<Func<Product, bool>> filtersDefinitions, IList<Operation> operations)
        {
            if (typeof(T) == typeof(Album))
            {
                return BuildAlbumReviewProcessor(sourceFile, outputsWriter, filtersDefinitions, operations);
            }
            throw new InvalidOperationException(string.Format("No builder is defined to manage review of type '{0}'", typeof(T)));
        }
    }
}