using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MIProgram.Core.Logging;

namespace MIProgram.Core.DataParsers
{
    public partial class StylesParser
    {
        #region private stuff

        private static int? TryRecognizeMusicType(ref string workingStyle, ref string beforeTypeString, ref string afterTypeString)
        {
            foreach (var musicTypeString in StyleDefinition.MusicGenresValues)
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
                    return StyleDefinition.MusicGenresValues.IndexOf(musicTypeString);
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

        private static IList<string> IntegrateExistingStylesAndAlterations(string textToParse, StyleDefinition styleDef, bool doStylesFirst)
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
                    if (StyleDefinition.MainStylesRepository.TryRetrieveValueIndex(part, ref idx))
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

                if (StyleDefinition.StyleAlterationsRepository.TryRetrieveValueIndex(part, ref idx))
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
                    if (StyleDefinition.MainStylesRepository.TryRetrieveValueIndex(part, ref idx))
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

        private static int? GetLastChanceMusicType(IList<string> mainStyles)
        {
            var possibleMusicStyles = new List<int>();
            var mainStylesToRetrieveAMusicStyle = mainStyles;

            if (mainStylesToRetrieveAMusicStyle.Count == 0)
            {
                return null;
            }
            possibleMusicStyles.AddRange(StyleDefinition.StylesAssociations.Where(x => mainStylesToRetrieveAMusicStyle.Contains(StyleDefinition.MainStylesValues[x.Key])).Select(y => y.Value).ToList());
            if (possibleMusicStyles.Count == 0)
            {
                return null;
            }
            int maxCount = 0;
            int? maxCountMusicTypeIdx = null;

            for (int i = 0; i < StyleDefinition.MusicGenresValues.Count; i++)
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
