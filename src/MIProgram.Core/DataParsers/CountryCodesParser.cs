﻿using System;
using System.Collections.Generic;
using MIProgram.Core.Logging;

namespace MIProgram.Core.DataParsers
{
    public class CountryCodesParser : IFieldParser<CountryDefinition, CountryDefinition>
    {
        public bool TryParse(string originCountry, int reviewId, ref CountryDefinition fieldDefinition)
        {
            try
            {
                if (string.IsNullOrEmpty(originCountry.Trim(new[] {' ', '-'})))
                {
                    fieldDefinition = new CountryDefinition(new List<int>());
                    return true;
                }

                var rawCountries = originCountry.Split('/');
                var countriesIdxs = new List<int>();

                foreach (var rawCountry in rawCountries)
                {
                    var countryIdx = CountryDefinition.CountryLabelsRepository.RetrieveValueIndex(rawCountry);
                    if (countryIdx.HasValue)
                    {
                        countriesIdxs.Add(countryIdx.Value);
                    }
                }

                if (countriesIdxs.Count > 0)
                {
                    fieldDefinition = new CountryDefinition(countriesIdxs);
                    return true;
                }
                var message = string.Format("'{0}' is not recognize as a valid country code", originCountry);
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du pays de l'artiste de la review  {0} : {1}", reviewId, message), ErrorLevel.Warning);
                return false;
        }
            catch (Exception e)
            {
                var message = string.Format("cannot parse originCountry '{0}'\n : {1}", originCountry, e.Message);
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors de l'extraction du pays de l'artiste de la review  {0} : {1}", reviewId, message), ErrorLevel.Warning);
                return false;
            }
        }

        public CountryDefinition ConvertToDestFieldDefinition(CountryDefinition fieldDefinition)
        {
            return fieldDefinition;
        }
    }
}