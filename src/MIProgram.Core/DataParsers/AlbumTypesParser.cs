using System;
using MIProgram.Core.Logging;

namespace MIProgram.Core.DataParsers
{
    public class AlbumTypesParser : IFieldParser<AlbumTypeDefinition, AlbumTypeDefinition>
    {
        public bool TryParse(string recordType, int reviewId, ref AlbumTypeDefinition fieldDefinition)
        {
            try
            {
                if (string.IsNullOrEmpty(recordType))
                {
                    throw new ArgumentNullException("recordType");
                }

                var albumTypeIdx = AlbumTypeDefinition.AlbumTypes.AddOrRetrieveValueIndex(recordType);

                if (!albumTypeIdx.HasValue)
                {
                    var message = string.Format("cannot parse recordType '{0}'", recordType);
                    Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du type de l'album de la review  {0} : {1}", reviewId, message), ErrorLevel.Info);
                    return false;
                }

                fieldDefinition = new AlbumTypeDefinition(albumTypeIdx.Value);
                return true;
            }
            catch (Exception e)
            {
                var message = string.Format("cannot parse recordType '{0}'\n : {1}", recordType, e.Message);
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du style de la review  {0} : {1}", reviewId, message), ErrorLevel.Info);
                return false;
            }
        }

        public AlbumTypeDefinition ConvertToDestFieldDefinition(AlbumTypeDefinition fieldDefinition)
        {
            return fieldDefinition;
        }
    }
}