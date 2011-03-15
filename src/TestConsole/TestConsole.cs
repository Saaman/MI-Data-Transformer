using System;
using System.Collections.Generic;
using System.IO;
using MIProgram.Core;
using MIProgram.Core.Creators;
using MIProgram.Core.Logging;
using MIProgram.Core.Cleaners;
using MIProgram.Model;
using MIProgram.Core.MIRecordsProviders;
using MIProgram.Core.Writers;
using FileHelpers;

namespace TestConsole
{
    public class TestConsole
    {
        const string _connectionString = "Data Source=localhost\\SQLEXPRESS; Integrated Security=SSPI;Initial Catalog=MetalImpact";
        const string _reviewCleanerCSV = "E:\\Metal Impact\\removals.mi";
        const string _xmlRootDir = "E:\\Metal Impact\\MIReviews\\";
        
        static ILogger _logger = new FileLogger("E:\\Metal Impact\\logs.txt");

        static void Main(string[] args)
        {
            TryExtract();
            //ExportAsXML();
        }

        static void ExportAsXML()
        {
            var reviews = GetReviews();
            var writer = new XmlFileWriter(_xmlRootDir);
            XMLCreator xmlCreator = new XMLCreator(writer, _xmlRootDir);
            xmlCreator.CreateXMLsFrom(reviews);
        }

        static void ExportAsExcel(string[] args)
        {
            var reviews = GetReviews();

            CSVCreator csvCreator = new CSVCreator("D:\\MetalImpact.csv");
            csvCreator.CreateCSVFrom(reviews);
        }

        static void TryExtract()
        {
            FileHelperEngine engine = new FileHelperEngine(typeof(MIDBRecord));
            var results = (IList<MIDBRecord>)engine.ReadFile("E:\\Metal Impact\\mi_reviews.csv");

        }

        #region methods

        private static IList<Review> GetReviews()
        {
            var reviewCleaner = FileReviewsCleaner.GetReviewCleanerFromFile(_reviewCleanerCSV);
            var miDBRecordsGetter = new CSVFileProvider(_connectionString);
            var reviewBuilder = new ReviewBuilder(_logger, reviewCleaner, new InMemoryCleaner(), null);
            var reviewsManager = new ReviewsManager(reviewBuilder);

            var results = miDBRecordsGetter.GetRecords();

            foreach (var result in results)
            {
                try
                {
                    reviewsManager.AddReviewFrom(result);
                }
                catch (Exception e)
                {
                    _logger.LogError(string.Format("Une erreur est survenue lors du parsing de la review {0} : {1}", result.Id, e.Message), ErrorLevel.Error);
                    continue;
                }
            }

            return reviewsManager.Reviews;
        }

        #endregion
    }
}
