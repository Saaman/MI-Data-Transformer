using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

namespace MIProgram.Core.DAL.Writers
{
    public class LocalFileWriter : IWriter
    {
        private readonly string _mainDirectory;

        public LocalFileWriter(string mainDirectory)
        {
            if(string.IsNullOrEmpty(mainDirectory))
                throw new ArgumentNullException("mainDirectory");
            
            if(!Directory.Exists(mainDirectory))
                Directory.CreateDirectory(mainDirectory);

            _mainDirectory = mainDirectory;
        }

        private string ComputeFilePath(string rootDir, string fileName)
        {
            string filePath = _mainDirectory;
            if (!string.IsNullOrEmpty(rootDir))
            {
                filePath = Path.Combine(filePath, rootDir);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            filePath = Path.Combine(filePath, fileName);
            return filePath;
        }

        public void WriteXML(XDocument xDoc, string fileNameWithoutExtension, string rootDir)
        {
            var filePath = ComputeFilePath(rootDir, string.Format("{0}.xml", fileNameWithoutExtension));

            xDoc.Save(filePath, SaveOptions.None);
        }

        public void WriteTextCollection(IList<string> collection, string fileNameWithoutExtension, string rootDir)
        {
            var filePath = ComputeFilePath(rootDir, string.Format("{0}.txt", fileNameWithoutExtension));
            var repository = new TextFileRepository(filePath);
            repository.WriteData(collection);
        }

        public void WriteSQL(string sqlString, string fileNameWithoutExtension, string rootDir)
        {
            var filePath = ComputeFilePath(rootDir, string.Format("{0}.sql", fileNameWithoutExtension));
            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(sqlString);
            }
            
        }

        public void WriteRB(string rbString, string fileNameWithoutExtension, string rootDir)
        {
            var filePath = ComputeFilePath(rootDir, string.Format("{0}.rb", fileNameWithoutExtension));
            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(rbString);
            }

        }

        public void WriteYAML(string yamlString, string fileNameWithoutExtension, string rootDir)
        {
            var filePath = ComputeFilePath(rootDir, string.Format("{0}.yml", fileNameWithoutExtension));
            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(yamlString);
            }

        }

        public void WriteCSV<T>(IList<T> collection, string fileNameWithoutExtension, string rootDir)
        {
            var filePath = ComputeFilePath(rootDir, string.Format("{0}.csv", fileNameWithoutExtension));
            var repository = new CSVFileRepository<T>(filePath);
            repository.WriteData(collection);
        }
    }
}