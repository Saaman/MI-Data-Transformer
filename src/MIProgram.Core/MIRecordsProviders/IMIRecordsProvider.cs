using System.Collections.Generic;
using MIProgram.DataAccess;

namespace MIProgram.Core.MIRecordsProviders
{
    public interface IMIRecordsProvider
    {
        IList<MIDBRecord> GetRecords();
        IList<MIDBRecord> GetAllRecords();
        /*void AddFilterOnIds(string pipedIds);
        void AddFilterOnReviewers(string pipedReviewers);*/
    }
}
