using System;
using MIProgram.Core.Model;

namespace MIProgram.Core
{
    public interface IExplodedReview<T> where T: Product
    {
        T AsDomainEntity { get;}
        int RecordId { get; }
        string ReviewBody { get; }
        string RecordTitle { get; }
        string ReviewerName { get; }
        DateTime RecordCreationDate { get; }

        void CleanTextUsing(Func<int, string, string> cleanTextMethod);
    }
}