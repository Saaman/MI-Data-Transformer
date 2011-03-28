using System.Collections.Generic;
using MIProgram.Core.DAL.Models;

namespace MIProgram.Core.MIRecordsProviders
{
    public interface IMIRecordsProvider
    {
        IList<MIDBRecord> GetAllRecords();
    }
}
