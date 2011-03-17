using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.Logging;
using MIProgram.DataAccess;

namespace MIProgram.Core.MIRecordsProviders
{
    public abstract class MIRecordsProvider<T> : IMIRecordsProvider
    {
        private readonly IList<KeyValuePair<IList<string>, Func<IList<string>, MIDBRecord, bool>>> _filters = new List<KeyValuePair<IList<string>, Func<IList<string>, MIDBRecord, bool>>>();

        public abstract IList<MIDBRecord> GetRecords();
        public abstract IList<MIDBRecord> GetAllRecords();

        protected IList<MIDBRecord> Filter(IList<MIDBRecord> records)
        {
            var results = records;
            foreach (var filter in _filters)
            {
                results = results.Where(x => filter.Value(filter.Key, x)).ToList();
            }

            return results;
        }

        public static bool TryBuildProvider(ILogger logger, T parameters, out IMIRecordsProvider provider)
        {
            throw new InvalidOperationException("La methode TryBuildProvider doit systématiquement être overridée. Vous n'êtes pas supposés passer dans ce code");
        }

        public void AddFilterOnIds(string pipedIds)
        {
            var ids = ExtractPipedValues(pipedIds);
            foreach (var id in ids)
            {
                int intId;
                if (!int.TryParse(id, out intId))
                {
                    throw new InvalidCastException(string.Format("The id '{0}' cannot be parsed as as valid number", id));
                }
            }
            _filters.Add(new KeyValuePair<IList<string>, Func<IList<string>, MIDBRecord, bool>>(ids, (x, y) => x.Contains(y.Id.ToString().ToUpperInvariant())));
        }

        public void AddFilterOnReviewers(string pipedReviewers)
        {
            if (string.IsNullOrEmpty(pipedReviewers))
            {
                return;
            }
            var rewievers = ExtractPipedValues(pipedReviewers);
            _filters.Add(new KeyValuePair<IList<string>, Func<IList<string>, MIDBRecord, bool>>(rewievers, (x, y) => x.Contains(y.ReviewerName.ToUpperInvariant())));
        }

        private static IList<string> ExtractPipedValues(string pipedValues)
        {
            var values = pipedValues.Split('|');
            var results = values.Select(value => value.ToUpperInvariant()).ToList();
            results = results.Where(x => !string.IsNullOrEmpty(x)).ToList();
            return results;
        }
    }
}