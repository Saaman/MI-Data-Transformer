using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.Model;

namespace MIProgram.Core.ProductRepositories
{
    public static class ProductFiltersBuilder
    {
        public static Func<Product, bool> NewFilterOnIds(string pipedIds)
        {
            if (string.IsNullOrEmpty(pipedIds))
            {
                return null;
            }

            IList<string> idsAsStrings = ExtractPipedValues(pipedIds);
            IList<int> ids;

            try
            {
                ids = idsAsStrings.Select(x => int.Parse(x)).ToList();
            }
            catch(Exception e)
            {
                throw new InvalidCastException(string.Format("Creating the filter on ids '{0}' failed : '{1}'", pipedIds, e));
            }

            return (x => ids .Contains(x.Id));
        }

        public static Func<Product, bool> NewFilterOnReviewers(string pipedReviewers)
        {
            if (string.IsNullOrEmpty(pipedReviewers))
            {
                return null;
            }
            var rewievers = ExtractPipedValues(pipedReviewers);
            return (x => rewievers.Contains(x.Artist.Reviewer.Name.ToUpperInvariant()));
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