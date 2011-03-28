using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.DAL;
using MIProgram.Core.Logging;
using MIProgram.DataAccess;

namespace MIProgram.Core
{
    public class ParsedValueRepository
    {
        public List<string> Values { get; private set; }
        private readonly IDictionary<string, string> _replacements;

        public ParsedValueRepository()
        {
            Values = new List<string>();
        }

        public ParsedValueRepository(string replacementValuesFileName)
        {
            _replacements = GetReplacementValuesFromFile(replacementValuesFileName);
        }

        public ParsedValueRepository(string originalValuesFileName, string replacementValuesFileName, bool addReplacementsValues)
            : this(replacementValuesFileName)
        {
            Values = GetOriginalValuesFromFile(originalValuesFileName);
            if (addReplacementsValues)
            {
                Values.AddRange(_replacements.Where(pair => !(string.IsNullOrEmpty(pair.Value) || Values.Contains(pair.Value))).Select(pair => pair.Value).Distinct().ToList());
            }
        }

        public ParsedValueRepository(List<string> originalValues, string replacementValuesFileName)
            : this(replacementValuesFileName)
        {
            Values = originalValues;
        }

        private static List<string> GetOriginalValuesFromFile(string fileName)
        {
            string message = string.Empty;
            var repository = new TextFileRepository(fileName);
            if (!repository.TestDbAccess(ref message))
            {
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors du chargement du dictionnaire '{0}': {1}", fileName, message), ErrorLevel.Error);
            }
            return repository.GetData().Select(x => x.ToUpperInvariant()).ToList();
        }
        private static IDictionary<string, string> GetReplacementValuesFromFile(string fileName)
        {
            string message = string.Empty;
            var repository = new CSVFileRepository<ReplacementEntity>(fileName);
            if (!repository.TestDbAccess(ref message))
            {
                Logging.Logging.Instance.LogError(string.Format("Une erreur est survenue lors du chargement du dictionnaire '{0}': {1}", fileName, message), ErrorLevel.Error);
            }
            var dic = repository.GetData().ToDictionary(x => x.ValueToReplace.ToUpperInvariant(), x => x.ReplacementValue.ToUpperInvariant());
            return dic;
        }

        public int? AddOrRetrieveValueIndex(string value)
        {
            var newValue = GetSafeValue(value);
            //var newValue = value.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(newValue))
            {
                return null;
            }
            if (!Values.Contains(newValue))
            {
                Values.Add(newValue);
            }

            return Values.IndexOf(newValue);
        }

        private string GetSafeValue(string value)
        {
            var result = value.Trim().ToUpperInvariant();
            if (_replacements.Keys.Contains(result))
            {
                return _replacements[result];
            }
            return result;
        }

        public bool TryRetrieveValueIndex(string value, ref int? idx)
        {
            var newValue = GetSafeValue(value);
            var result = newValue != value;
            if (string.IsNullOrEmpty(newValue))
            {
                idx = null;
                return result;
            }
            if (Values.Contains(newValue))
            {
                idx = Values.IndexOf(newValue);
                return true;
            }
            return result;
        }

        public int? RetrieveValueIndex(string value)
        {
            int? idx = null;
            TryRetrieveValueIndex(value, ref idx);
            return idx;
        }

        public string ReplaceReplacements(string workingStyle)
        {
            string result = workingStyle.ToUpperInvariant();
            foreach (var replacement in _replacements)
            {
                if (result.Contains(replacement.Key.ToUpperInvariant()))
                {
                    result = result.Replace(replacement.Key.ToUpperInvariant(), replacement.Value.ToUpperInvariant());
                }
            }
            return result;
        }
    }
}