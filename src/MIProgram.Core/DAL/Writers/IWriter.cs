using System.Collections.Generic;
using System.Xml.Linq;

namespace MIProgram.Core.Writers
{
    public interface IWriter
    {
        void WriteXML(XDocument xDoc, string fileNameWithoutExtension, string rootDir);
        void WriteTextCollection(IList<string> collection, string fileNameWithoutExtension, string rootDir);
        void WriteCSV<T>(IList<T> collection, string fileNameWithoutExtension, string rootDir);
        void WriteSQL(string sqlString, string fileNameWithoutExtension, string rootDir);
    }
}
