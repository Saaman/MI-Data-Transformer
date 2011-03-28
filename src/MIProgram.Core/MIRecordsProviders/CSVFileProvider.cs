using System.Collections.Generic;
using MIProgram.Core.DAL;
using MIProgram.Core.DAL.Models;
using MIProgram.Core.Logging;

namespace MIProgram.Core.MIRecordsProviders
{
    public class CSVFileProvider : IMIRecordsProvider
    {
        private readonly CSVFileRepository<MIDBRecord> _miRecordsRepository;

        private CSVFileProvider(string fileName)
        {
            _miRecordsRepository = new CSVFileRepository<MIDBRecord>(fileName);
        }

        public IList<MIDBRecord> GetAllRecords()
        {
            return _miRecordsRepository.GetData();
        }

        public static bool TryBuildProvider(string fileName, out IMIRecordsProvider provider)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                provider = null;
                return false;
            }

            string message = null;
            var res = new CSVFileRepository<MIDBRecord>(fileName).TestDbAccess(ref message);
            if (!res)
            {
                Logging.Logging.Instance.LogError(message, ErrorLevel.Error);
                provider = null;
                return false;
            }

            provider = new CSVFileProvider(fileName);
            return true;
        }
    }
}
