using MIProgram.DataAccess;
using MIProgram.Model;

namespace MIProgram.Core
{
    public interface IReviewExploder<T> where T: Product
    {
        IExplodedReview<T> ExplodeReviewFrom(MIDBRecord record);
    }
}