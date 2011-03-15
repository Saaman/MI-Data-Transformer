using System;
using System.Collections.Generic;
using System.IO;
using FileHelpers;

namespace MIProgram.DataAccess
{
    public class CSVFileRepository<T> : IFileRepository<IList<T>>
    {
        private readonly string _fileName;
        private readonly FileHelperEngine _engine;

        public CSVFileRepository(string fileName)
        {
            _fileName = fileName;
            _engine = new FileHelperEngine(typeof(T));
        }

        public bool TestDbAccess(ref string message)
        {
            if (!File.Exists(_fileName))
            {
                message = string.Format("Le fichier source '{0}' n'existe pas", _fileName);
                return false;
            }

            try
            {
                var engine = new FileHelperEngine(typeof(T));
                engine.ReadFile(_fileName);
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }

            return true;
        }

        public IList<T> GetData()
        {
            
            return _engine.ReadFile(_fileName) as IList<T>;
        }

        public void WriteData(IList<T> data)
        {
            _engine.WriteFile(_fileName, data);
        }
    }
}