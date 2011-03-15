using System.Collections.Generic;
using System.Linq;
using MIProgram.Model.Extensions;

namespace MIProgram.Core.DataParsers
{
    public class StyleDefinition : IFieldDefinition
    {
        private const int MaxNumberOfMusicTypes = 2;
        private const int MaxNumberOfMainStyles = 2;
        private const int MaxNumberOfStyleAlterations = 2;

        #region Dictionnaries

        public static readonly IList<KeyValuePair<int, int>> StylesAssociations = new List<KeyValuePair<int, int>>();

        public static IList<string> MusicGenresValues { get { return MusicGenresRepository.Values; } }
        public static IList<string> MainStylesValues { get { return MainStylesRepository.Values; } }
        public static IList<string> StyleAlterationsValues { get { return StyleAlterationsRepository.Values; } }

        internal static readonly ParsedValueRepository MusicGenresRepository = new ParsedValueRepository(Constants.MusicGenresFileName, Constants.MusicGenresReplacementsFileName, false);
        internal static readonly ParsedValueRepository MainStylesRepository = new ParsedValueRepository(Constants.MainStylesFileName, Constants.MainStylesReplacementsFileName, true);
        internal static readonly ParsedValueRepository StyleAlterationsRepository = new ParsedValueRepository(Constants.StyleAlterationsFileName, Constants.StyleAlterationsReplacementsFileName, true);

        #endregion

        private IList<int> _musicTypesIdxs;
        private readonly IList<int> _mainStylesIdxs;
        private readonly IList<int> _styleAlterationIdxs;
        public int Complexity
        {
            get { return _musicTypesIdxs.Count + _mainStylesIdxs.Count*2 + _styleAlterationIdxs.Count*3; }
        }

        public IList<int> MusicTypesIdxs { get { return _musicTypesIdxs; } }
        public IList<int> MainStylesIdxs { get { return _mainStylesIdxs; } }
        public IList<int> StyleAlterationsIdxs { get { return _styleAlterationIdxs; } }

        public IList<string> MusicTypes { get { return _musicTypesIdxs.Select(x => MusicGenresValues[x]).ToList(); } }
        public IList<string> MainStyles { get { return _mainStylesIdxs.Select(x => (MainStylesValues[x])).ToList(); } }
        public IList<string> StyleAlterations { get { return _styleAlterationIdxs.Select(x => (StyleAlterationsValues[x])).ToList(); } }

        #region ctor

        public StyleDefinition(IList<int> musicTypesIdxs)
        {
            if (musicTypesIdxs.Count > MaxNumberOfMusicTypes)
            {
                throw new InvalidMusicStyleParsingException(string.Format("Music style contains {0} music types, {1} maximum is allowed", musicTypesIdxs.Count, MaxNumberOfMusicTypes));
            }
            _musicTypesIdxs = musicTypesIdxs;
            _styleAlterationIdxs = new List<int>();
            _mainStylesIdxs = new List<int>();
        }

        public StyleDefinition(IList<int> musicTypesIdxs, IList<int> subStylesIdxs): this(musicTypesIdxs)
        {
            if (subStylesIdxs.Count > MaxNumberOfMainStyles)
            {
                throw new InvalidMusicStyleParsingException(string.Format("Music style contains {0} substyles, {1} maximum is allowed", subStylesIdxs.Count, MaxNumberOfMainStyles));
            }
            _mainStylesIdxs = subStylesIdxs;
        }

        #endregion

        #region public Methods

        public static string ReplaceMusicGenresReplacement(string workingStyle)
        {
            return MusicGenresRepository.ReplaceReplacements(workingStyle);
        }

        public static void AttachMainStyleToMusicType(int mainStyleIdx, int musicTypeIdx)
        {
            if (StylesAssociations.Any(x => x.Key == mainStyleIdx && x.Value == musicTypeIdx))
            {
                return;
            }
            StylesAssociations.Add(new KeyValuePair<int,int>(mainStyleIdx, musicTypeIdx));
        }

        public void AttachMusicType(int musicTypeIdx)
        {
            _musicTypesIdxs = new List<int> { musicTypeIdx };
        }

        public string RebuildFromParsedValuesRepository()
        {
            var result = string.Empty;

            result = _mainStylesIdxs.Aggregate(result, (current, mainStyleIdx) => current + MainStylesValues[mainStyleIdx].ToCamelCase() + "/");
            result = result.TrimEnd(new[] { ' ', '/' });

            if(!string.IsNullOrEmpty(result))
            {
                result += " ";
            }

            result = _musicTypesIdxs.Aggregate(result, (current, musicType) => current + MusicGenresValues[musicType] + "/");
            result = result.TrimEnd(new[] {' ', '/'});

            result += " ";

            result = _styleAlterationIdxs.Aggregate(result, (current, styleAlteration) => current + StyleAlterationsValues[styleAlteration].ToCamelCase() + " ");
            result = result.TrimEnd(new[] { ' ', '/' });            

            return result;
        }

        public void ThrowIfAddMainStyleIsForbidden()
        {
            if (_mainStylesIdxs.Count == MaxNumberOfMainStyles)
            {
                throw new InvalidMusicStyleParsingException(string.Format("you cannot have {0} main styles, {1} maximum is allowed", MaxNumberOfMusicTypes + 1, MaxNumberOfMusicTypes));
            }
        }

        public void AddMainStyleIdx(int? idx)
        {
            if (!idx.HasValue)
            {
                return;
            }
            ThrowIfAddMainStyleIsForbidden();
            if (_mainStylesIdxs.Contains(idx.Value))
            {
                return;
            }
            _mainStylesIdxs.Add(idx.Value);
        }

        public void ThrowIfAddStyleAlterationIsForbidden()
        {
            if (_styleAlterationIdxs.Count == MaxNumberOfStyleAlterations)
            {
                throw new InvalidMusicStyleParsingException(string.Format("you cannot have {0} style alterations, {1} maximum is allowed", MaxNumberOfStyleAlterations + 1, MaxNumberOfStyleAlterations));
            }
        }

        public StyleDefinition AddStyleAlterationIdx(int? idx)
        {
            if (!idx.HasValue)
            {
                return this;
            }

            if (_styleAlterationIdxs.Contains(idx.Value))
            {
                return this;
            }

            ThrowIfAddStyleAlterationIsForbidden();

            _styleAlterationIdxs.Add(idx.Value);
            return this;
        }

        #endregion
    }


    #region additionnal classes

    public class InvalidMusicStyleParsingException : System.Exception
    {
        public InvalidMusicStyleParsingException(string message)
            : base(message)
        { }
    }

    #endregion
}
