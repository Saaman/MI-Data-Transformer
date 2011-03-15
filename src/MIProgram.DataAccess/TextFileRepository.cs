using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MIProgram.DataAccess
{
    public class TextFileRepository : IFileRepository<IList<string>>
    {
        private readonly string _fileName;

        public TextFileRepository(string fileName)
        {
            _fileName = fileName;
        }

        public bool TestDbAccess(ref string message)
        {
            if (!File.Exists(_fileName))
            {
                message = string.Format("Le fichier source '{0}' n'existe pas", _fileName);
                return false;
            }
            return true;
        }

        public IList<string> GetData()
        {
            var result = new List<string>();
            using(var sr = new StreamReader(_fileName, Encoding.Unicode))
            {
                while (!sr.EndOfStream)
                {
                    result.Add(sr.ReadLine());
                }
            }
            return result;
        }

        public void WriteData(IList<string> data)
        {
            using (var sw = new StreamWriter(_fileName, false, Encoding.Unicode))
            {
                foreach(var line in data)
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}