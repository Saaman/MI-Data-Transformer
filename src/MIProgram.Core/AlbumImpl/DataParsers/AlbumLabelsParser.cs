using System;
using MIProgram.Core.AlbumImpl.LocalRepositories;
using MIProgram.Core.Logging;
using MIProgram.Core.Model;

namespace MIProgram.Core.AlbumImpl.DataParsers
{
    public class AlbumLabelsParser
    {
        public bool TryParse(string label, string vendor, DateTime reviewDate, int reviewId, ref LabelVendor labelVendor)
        {
            try
            {
                if (string.IsNullOrEmpty(label))
                {
                    throw new ArgumentNullException("label");
                }

                labelVendor = AlbumLabelsRepository.CreateOrUpdate(label, vendor ?? string.Empty , reviewDate);
                return true;
            }
            catch (Exception e)
            {
                var message = string.Format("cannot parse label and vendor '{0}/{1}'\n : {2}", label, vendor, e.Message);
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du style de la review  {0} : {1}", reviewId, message), ErrorLevel.Info);
                return false;
            }
        }
    }
}