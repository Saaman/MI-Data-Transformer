﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace MIProgram.Core.DAL.Writers
{
    public interface IWriter
    {
        void WriteXML(XDocument xDoc, string fileNameWithoutExtension, string rootDir);
        void WriteTextCollection(IList<string> collection, string fileNameWithoutExtension, string rootDir);
        void WriteCSV<T>(IList<T> collection, string fileNameWithoutExtension, string rootDir);
        void WriteSQL(string sqlString, string fileNameWithoutExtension, string rootDir);
        void WriteRB(string railsString, string fileNameWithoutExtension, string rootDir);
        void WriteYAML(string railsString, string fileNameWithoutExtension, string rootDir);
        void CleanFile(string fileNameWithoutExtension, string rootDir);
    }
}
