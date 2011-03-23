using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MIProgram.Core.Logging;
using MIProgram.DataAccess;

namespace MIProgram.Core.DataParsers
{
    public static class StylesRepository
    {
        private static IList<string> _splittingStrings;
        private static IEnumerable<string> SplittingStrings
        {
            get
            {
                if (_splittingStrings == null)
                {
                    var repository = new TextFileRepository(Constants.SplittingStringsFilePath);
                    _splittingStrings = repository.GetData().Select(x => x.ToUpperInvariant()).ToList();
                }
                return _splittingStrings;
            }
        }

        static StylesRepository()
        {
            MusicGenresRepository = new ParsedValueRepository(Constants.MusicGenresFileName, Constants.MusicGenresReplacementsFileName, false);
            MainStylesRepository = new ParsedValueRepository(Constants.MainStylesFileName, Constants.MainStylesReplacementsFileName, true);
            StyleAlterationsRepository = new ParsedValueRepository(Constants.StyleAlterationsFileName, Constants.StyleAlterationsReplacementsFileName, true);
            StylesAssociations = new List<KeyValuePair<int, int>>();
        }

        #region Attributes

        public static IList<KeyValuePair<int, int>> StylesAssociations { get; private set; }

        public static ParsedValueRepository MainStylesRepository { get; private set; }

        public static ParsedValueRepository StyleAlterationsRepository { get; private set; }

        public static ParsedValueRepository MusicGenresRepository { get; private set; }

        #endregion

        #region Methods

        public static int? TryRecognizeMusicType(ref string workingStyle, ref string beforeTypeString, ref string afterTypeString)
        {
            foreach (var musicTypeString in MusicGenresRepository.Values)
            {
                //var regex = new Regex(string.Format("([^a-zA-Z]|^){0}([^a-zA-Z]|$)", musicTypeString.ToUpperInvariant()));
                var regex = new Regex(string.Format("{0}([^a-zA-Z]|$)", musicTypeString.ToUpperInvariant()));
                if (regex.IsMatch(workingStyle.ToUpperInvariant()))
                {
                    int idx = regex.Match(workingStyle.ToUpperInvariant()).Index;
                    /*if (workingStyle.ToUpperInvariant()[idx] != musicTypeString.ToUpperInvariant()[0])
                    {
                        idx++;
                    }*/
                    beforeTypeString = workingStyle.Substring(0, idx).Trim(new[] { ' ', '-' });
                    afterTypeString = workingStyle.Remove(0, idx + musicTypeString.Length).Trim(new[] { ' ', '-' });
                    workingStyle = workingStyle.Remove(idx, musicTypeString.Length).Trim(new[] { ' ', '-' });
                    return MusicGenresRepository.Values.IndexOf(musicTypeString);
                }
                /*if (workingStyle.ToUpperInvariant().Contains(musicTypeString.ToUpperInvariant()))
                {
                    int idx = workingStyle.ToUpperInvariant().IndexOf(musicTypeString.ToUpperInvariant());
                    beforeTypeString = workingStyle.Substring(0, idx).Trim(new[] { ' ', '-' });
                    afterTypeString = workingStyle.Remove(0, idx + musicTypeString.Length).Trim(new[] { ' ', '-' });
                    workingStyle = workingStyle.Remove(idx, musicTypeString.Length).Trim(new[] { ' ', '-' });
                    return StyleDefinition.MusicGenresValues.IndexOf(musicTypeString);
                }*/
            }
            return null;
        }

        public static IList<string> IntegrateExistingStylesAndAlterations(string textToParse, StyleDefinition styleDef, bool doStylesFirst)
        {
            if (string.IsNullOrEmpty(textToParse))
            {
                return new List<string>();
            }

            var styleParts = new List<string>();
            styleParts.AddRange(textToParse.ToUpperInvariant().Split(SplittingStrings.ToArray(), StringSplitOptions.RemoveEmptyEntries));
            styleParts = styleParts.Where(x => SplittingStrings.Where(y => y.Trim().ToUpperInvariant().Contains(x.Trim().ToUpperInvariant())).Count() == 0).ToList();
            var partsToRemove = new List<string>();

            int? idx = null;

            foreach (var part in styleParts)
            {
                #region search main style
                if (doStylesFirst)
                {
                    if (MainStylesRepository.TryRetrieveValueIndex(part, ref idx))
                    {
                        partsToRemove.Add(part);
                        if (idx.HasValue)
                        {
                            try
                            {
                                styleDef.ThrowIfAddMainStyleIsForbidden();
                            }
                            catch (InvalidMusicStyleParsingException)
                            {
                                var message = string.Format("while parsing style part '{0}' we detect too many main styles. string '{1}' will be ignored", textToParse, part);
                                Logging.Logging.Instance.LogError(string.Format("A turnaround will happen : {0}", message), ErrorLevel.Info);
                                continue;
                            }

                            styleDef.AddMainStyleIdx(idx.Value);
                        }
                        continue;
                    }
                }
                #endregion

                #region search style alteration

                if (StyleAlterationsRepository.TryRetrieveValueIndex(part, ref idx))
                {
                    partsToRemove.Add(part);
                    if (idx.HasValue)
                    {
                        try
                        {
                            styleDef.ThrowIfAddStyleAlterationIsForbidden();
                        }
                        catch (InvalidMusicStyleParsingException)
                        {
                            var message = string.Format("while parsing style part '{0}' we detect too many style alterations. string '{1}' will be ignored", textToParse, part);
                            Logging.Logging.Instance.LogError(string.Format("A workaround will happen  : {0}", message), ErrorLevel.Info);
                            continue;
                        }
                        styleDef.AddStyleAlterationIdx(idx.Value);
                    }
                    continue;
                }
                #endregion

                #region search main style
                if (!doStylesFirst)
                {
                    if (MainStylesRepository.TryRetrieveValueIndex(part, ref idx))
                    {
                        partsToRemove.Add(part);
                        if (idx.HasValue)
                        {
                            try
                            {
                                styleDef.ThrowIfAddMainStyleIsForbidden();
                            }
                            catch (InvalidMusicStyleParsingException)
                            {
                                var message = string.Format("while parsing style part '{0}' we detect too many main styles. string '{1}' will be ignored", textToParse, part);
                                Logging.Logging.Instance.LogError(string.Format("A turnaround will happen : {0}", message), ErrorLevel.Info);
                                continue;
                            }

                            styleDef.AddMainStyleIdx(idx.Value);
                        }
                        continue;
                    }
                }
                #endregion
            }

            foreach (var part in partsToRemove)
            {
                styleParts.Remove(part);
            }
            return styleParts;
        }

        public static int? GetLastChanceMusicType(IList<string> mainStyles)
        {
            var possibleMusicStyles = new List<int>();
            var mainStylesToRetrieveAMusicStyle = mainStyles;

            if (mainStylesToRetrieveAMusicStyle.Count == 0)
            {
                return null;
            }
            possibleMusicStyles.AddRange(StylesAssociations.Where(x => mainStylesToRetrieveAMusicStyle.Contains(MainStylesRepository.Values[x.Key])).Select(y => y.Value).ToList());
            if (possibleMusicStyles.Count == 0)
            {
                return null;
            }
            int maxCount = 0;
            int? maxCountMusicTypeIdx = null;

            for (int i = 0; i < MusicGenresRepository.Values.Count; i++)
            {
                int currentI = i;
                if (possibleMusicStyles.Where(x => x == currentI).Count() == 0)
                {
                    continue;
                }

                if (possibleMusicStyles.Where(x => x == currentI).Count() == maxCount)
                {
                    maxCountMusicTypeIdx = null;
                    continue;
                }

                if (possibleMusicStyles.Where(x => x == currentI).Count() > maxCount)
                {
                    maxCountMusicTypeIdx = currentI;
                    maxCount = possibleMusicStyles.Where(x => x == currentI).Count();
                }
            }

            if (maxCountMusicTypeIdx.HasValue)
            {
                return maxCountMusicTypeIdx.Value;
            }
            return possibleMusicStyles[0];
        }

        #endregion
    }
}