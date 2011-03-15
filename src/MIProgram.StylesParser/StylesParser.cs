using System;
using MIProgram.Core.Logging;
using MIProgram.WorkingModel;

namespace MIProgram.StylesParser
{
    public class StylesParser : IFieldParser<StyleDefinition>
    {
        public bool TryParse(Review review, ref StyleDefinition fieldDefinition)
        {
            return TryParse(review.Album.MusicType, review.Id, ref fieldDefinition);
        }

        protected bool TryParse(string style, int reviewId, ref StyleDefinition fieldDefinition)
        {
            try
            {
                var workingStyle = style.Replace("metal", "");
                workingStyle = workingStyle.Replace("Metal", "");
                workingStyle = workingStyle.Replace("Métal", "");
                workingStyle = workingStyle.Replace("métal", "");
                workingStyle = workingStyle.Trim();

                if (string.IsNullOrEmpty(style))
                {
                    throw new ArgumentNullException("style");
                }

                var styleParts = workingStyle.Split(new[] {" ", ",", ".", "/", "\\"}, StringSplitOptions.None);



                if (styleParts.Length > 3 || styleParts.Length < 1)
                {
                    var message = string.Format("cannot parse style '{0}' : it contains {1} parts, 3 maximum are allowed", style, styleParts.Length);
                    Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du style de la review  {0} : {1}", reviewId, message), ErrorLevel.Info);
                    return false;
                }

                int? secondStyleIdx = null;
                int? styleAlterationIdx = null;


                int? mainStyleIdx = StyleDefinition.MainStyles.AddOrRetrieveValueIndex(styleParts[0]);
                if (styleParts.Length > 1)
                {
                    secondStyleIdx = StyleDefinition.MainStyles.AddOrRetrieveValueIndex(styleParts[1]);
                    if (styleParts.Length > 2)
                    {
                        styleAlterationIdx = StyleDefinition.StyleAlterations.AddOrRetrieveValueIndex(styleParts[2]);
                    }
                }

                fieldDefinition = new StyleDefinition(mainStyleIdx, secondStyleIdx, styleAlterationIdx);
                return true;
            }
            catch (Exception e)
            {
                var message = string.Format("cannot parse style '{0}'\n : {1}", style, e.Message);
                Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du style de la review  {0} : {1}", reviewId, message), ErrorLevel.Info);
                return false;
            }
        }
    }
}
