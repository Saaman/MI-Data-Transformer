using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace MIProgram.Core
{
    public static class Constants
    {
        public static readonly string DictionnariesDirectoryPath = ConfigurationManager.AppSettings["DictionnariesPath"];
        public static readonly string SQLPath = ConfigurationManager.AppSettings["SQLPath"];
        public static readonly string DeezerOperationsOutputDirectoryPath = ConfigurationManager.AppSettings["DeezerDir"];
        public static readonly string MigrationOperationsOutputDirectoryPath = ConfigurationManager.AppSettings["MigrationDir"];
        public static readonly string FieldsExtractionsOutputDirectoryPath = ConfigurationManager.AppSettings["FieldsExtractionsDir"];

        public static readonly string SplittingStringsFilePath = Path.Combine(DictionnariesDirectoryPath, "splittingStrings.txt");
        public static readonly string MusicGenresFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "musicGenres.txt");
        public static readonly string MusicGenresReplacementsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "musicGenresReplacements.csv");
        public static readonly string MainStylesFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "mainStyles.txt");
        public static readonly string MainStylesReplacementsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "mainStylesReplacements.csv");
        public static readonly string StyleAlterationsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "styleAlterations.txt");
        public static readonly string StyleAlterationsReplacementsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "styleAlterationsReplacements.csv");

        public static readonly string MIRemovalsFilePath = BuildBinPath("miRemovals.mi");
        public static readonly string LogFilePath = BuildBinPath("logFile.txt");

        public static string BuildBinPath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(path))
            {
                throw new FileNotFoundException("The executing path of the programm cannot be retrieved");
            }
            return Path.Combine(path, fileName);
        }
    }
}
