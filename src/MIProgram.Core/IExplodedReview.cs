using MIProgram.Model;

namespace MIProgram.Core
{
    public interface IExplodedReview<T> where T: Product
    {
        T AsDomainEntity { get;}
    }
}