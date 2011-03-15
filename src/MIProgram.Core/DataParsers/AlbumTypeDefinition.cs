using System.Collections.Generic;
using System.Configuration;
using System.IO;
using MIProgram.Model.Extensions;

namespace MIProgram.Core.DataParsers
{
    public class AlbumTypeDefinition : IFieldDefinition
    {
        private static readonly string AlbumTypesFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "albumTypes.txt");
        private static readonly string AlbumTypesReplacementsFileName = Path.Combine(ConfigurationManager.AppSettings["DictionnariesPath"], "albumTypesReplacements.csv");

        private readonly int _albumTypeIdx;
        public string AlbumType {get { return AlbumTypes.Values[_albumTypeIdx];}}

        public static IList<string> AlbumTypesValues
        {
            get { return AlbumTypes.Values; }
        }

        internal static readonly ParsedValueRepository AlbumTypes = new ParsedValueRepository(AlbumTypesFileName, AlbumTypesReplacementsFileName, true);
        
        public AlbumTypeDefinition(int albumTypeIdx)
        {
            _albumTypeIdx = albumTypeIdx;
        }

        public string RebuildFromParsedValuesRepository()
        {
            return AlbumTypes.Values[_albumTypeIdx].ToCamelCase();
        }
    }
}