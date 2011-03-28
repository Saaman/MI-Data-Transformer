using MIProgram.Core.DAL.Models;
using MIProgram.Core.Model;

namespace MIProgram.Core
{
    public interface IReviewExploder<T> where T: Product
    {
        IExplodedReview<T> ExplodeReviewFrom(MIDBRecord record);
    }
}