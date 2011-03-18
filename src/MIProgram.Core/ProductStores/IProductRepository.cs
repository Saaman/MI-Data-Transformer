using System.Collections.Generic;
using MIProgram.Model;

namespace MIProgram.Core.ProductStores
{
    public interface IProductRepository<T> where T: Product
    {
        IList<Artist> Artists { get; }
        IList<Reviewer> Reviewers { get; }
        IList<T> Products { get; }
        IList<T> FilteredProducts { get; }
        void Add(IExplodedReview<T> review);
    }
}