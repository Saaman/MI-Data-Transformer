using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.Logging;
using MIProgram.DataAccess;

namespace MIProgram.Core.DataParsers
{
    public partial class StylesParser : IFieldParser<StyleDefinition, StyleDefinition>
    {
        public bool TryParse(string style, int reviewId, ref StyleDefinition fieldDefinition)
        {
            if (string.IsNullOrEmpty(style))
            {
                throw new ArgumentNullException("style");
            }

            var workingStyle = StyleDefinition.ReplaceMusicGenresReplacement(style.Trim(new[] { ',', ' ', '\t', ';', '.' }));

            try
            {
                var musicTypesIdxs = new List<int>();

                //reconnaissances des types de musique
                string beforeTypeString = workingStyle;
                string afterTypeString = null;
                string workingStyleForMusicTypeRecognition = workingStyle;

                int? musicTypeIdx;
                do
                {
                    musicTypeIdx = TryRecognizeMusicType(ref workingStyleForMusicTypeRecognition, ref beforeTypeString, ref afterTypeString);
                    if (musicTypeIdx.HasValue)
                    {
                        if (!musicTypesIdxs.Contains(musicTypeIdx.Value))
                        {
                            musicTypesIdxs.Add(musicTypeIdx.Value);
                        }
                    }
                } while (musicTypeIdx.HasValue);

                fieldDefinition = new StyleDefinition(musicTypesIdxs);

                var beforeTypeRemainingParts = IntegrateExistingStylesAndAlterations(beforeTypeString, fieldDefinition, true);
                var afterTypeRemainingParts = IntegrateExistingStylesAndAlterations(afterTypeString, fieldDefinition, false);

                //Dernière chance pour trouver un type de musique
                if (musicTypesIdxs.Count == 0)
                {
                    var lastChanceMusicType = GetLastChanceMusicType(fieldDefinition.MainStyles);
                    if (lastChanceMusicType.HasValue)
                    {
                        fieldDefinition.AttachMusicType(lastChanceMusicType.Value);
                    }
                    else if (fieldDefinition.MainStyles.Count > 0)
                    {
                        var message = string.Format("while parsing style '{0}' we can't detect any music genre. We assume that 'Metal' is the main genre of this album", style);
                        Logging.Logging.Instance.LogError(string.Format("A turnaround will happen on review {0} : {1}", reviewId, message), ErrorLevel.Info);
                        fieldDefinition.AttachMusicType(0);
                    }
                    else
                    {
                        var message = string.Format("cannot parse style '{0}' : no existing music type could have been recognized", style);
                        Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du style de la review  {0} : {1}", reviewId, message), ErrorLevel.Warning);
                        return false;
                    }
                }

                // Creation des nouveaux styles
                foreach (var newMainStyle in beforeTypeRemainingParts)
                {
                    try
                    {
                        fieldDefinition.ThrowIfAddMainStyleIsForbidden();
                    }
                    catch (InvalidMusicStyleParsingException)
                    {
                        var message = string.Format("while parsing style '{0}' we detect too many main styles. string '{1}' will be considered as a style alteration", style, newMainStyle);
                        Logging.Logging.Instance.LogError(string.Format("A turnaround will happen on review {0} : {1}", reviewId, message), ErrorLevel.Info);
                        afterTypeRemainingParts.Insert(0, newMainStyle);
                        continue;
                    }
                    fieldDefinition.AddMainStyleIdx(StyleDefinition.MainStylesRepository.AddOrRetrieveValueIndex(newMainStyle));
                }

                //Attachement des styles
                if (fieldDefinition.MusicTypes.Count == 1)
                {
                    foreach (var mainStyleIdx in fieldDefinition.MainStylesIdxs)
                    {
                        StyleDefinition.AttachMainStyleToMusicType(mainStyleIdx, fieldDefinition.MusicTypesIdxs[0]);
                    }
                }

                // Creation des nouvelles altérations
                foreach (var newAlteration in afterTypeRemainingParts)
                {
                    try
                    {
                        fieldDefinition.ThrowIfAddStyleAlterationIsForbidden();
                    }
                    catch (InvalidMusicStyleParsingException)
                    {
                        var message = string.Format("while parsing style '{0}' we detect too many style alterations. string '{1}' will be abandonned", style, newAlteration);
                        Logging.Logging.Instance.LogError(string.Format("A workaround will happen on review {0} : {1}", reviewId, message), ErrorLevel.Info);
                        continue;
                    }
                    fieldDefinition.AddStyleAlterationIdx(StyleDefinition.StyleAlterationsRepository.AddOrRetrieveValueIndex(newAlteration));
                }

                return true;
            }
            catch (Exception e)
            {
                var message = string.Format("cannot parse style '{0}'\n : {1}", style, e.Message);
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du style de la review  {0} : {1}", reviewId, message), ErrorLevel.Warning);
                return false;
            }
        }

        public StyleDefinition ConvertToDestFieldDefinition(StyleDefinition fieldDefinition)
        {
            return fieldDefinition;
        }

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
    }
}
