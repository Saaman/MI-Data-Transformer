using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using System.IO;
using MIProgram.WorkingModel;

namespace MIProgram.Core.Creators
{
    public class CSVCreator
    {
        private readonly string _filePath;
        
        public CSVCreator(string filePath)
        {
            if (!Directory.Exists(Path.GetPathRoot(filePath)))
            {
                throw new DirectoryNotFoundException(string.Format("The destination folder {0} does not exist.", Path.GetPathRoot(filePath)));
            }

            _filePath = filePath;
        }

        public void CreateCSVFrom(IList<Review> reviews)
        {
            IList<CSVRecord> records = reviews.Select(review => new CSVRecord(review)).ToList();

            var engine = new FileHelperEngine(typeof(CSVRecord)) {HeaderText = CSVRecord.CSVHeader};
            engine.WriteFile(_filePath, records);
        }
    }
}
