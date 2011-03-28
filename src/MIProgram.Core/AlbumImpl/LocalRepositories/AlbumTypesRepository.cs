using System.Configuration;
using System.IO;
using MIProgram.Core.Extensions;

namespace MIProgram.Core.AlbumImpl.LocalRepositories
{
    public static class AlbumTypesRepository
    {
        private static readonly string AlbumTypesFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "albumTypes.txt");
        private static readonly string AlbumTypesReplacementsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "albumTypesReplacements.csv");

        private static readonly ParsedValueRepository _albumTypes;

        static AlbumTypesRepository()
        {
            _albumTypes = new ParsedValueRepository(AlbumTypesFileName, AlbumTypesReplacementsFileName, true);
        }

        public static ParsedValueRepository Repo
        {
            get { return _albumTypes; }
        }

        public static string ToDomainObject(int albumTypeIdx)
        {
            return _albumTypes.Values[albumTypeIdx].ToCamelCase();
        }
    }
}