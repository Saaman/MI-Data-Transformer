using System;
using MIProgram.Core;
using MIProgram.Core.AlbumImpl;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Model;

namespace MetalImpactApp
{
    public class ReviewProcessorBuilder
    {
        public ReviewProcessor<T> BuildFor<T>(string sourceFile) where T: Product
        {
            if(typeof(T) == typeof(Album))
            {
                return BuildAlbumReviewProcessor(sourceFile) as ReviewProcessor<T>;
            }
            throw new InvalidOperationException(string.Format("No builder is defined to manage review of type '{0}'", typeof(T)));
        }

        private static AlbumReviewProcessor BuildAlbumReviewProcessor(string sourceFile)
        {
            IMIRecordsProvider provider;
            CSVFileProvider.TryBuildProvider(sourceFile, out provider);
            var reviewExploder = new AlbumReviewExploder();
            var reviewProcessor = new AlbumReviewProcessor(provider, reviewExploder);
            return reviewProcessor;
        }
    }
}