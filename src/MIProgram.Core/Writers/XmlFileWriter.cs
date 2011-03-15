using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace MIProgram.Core.Writers
{
    public class XmlFileWriter : IXmlWriter
    {
        private string _mainDirectory;

        public XmlFileWriter(string mainDirectory)
        {
            if(string.IsNullOrEmpty(mainDirectory))
                throw new ArgumentNullException("mainDirectory");
            
            if(!Directory.Exists(mainDirectory))
                Directory.CreateDirectory(mainDirectory);

            _mainDirectory = mainDirectory;
        }

        public void Write(XDocument xDoc, string fileNameWithoutExtension, string rootDir)
        {
            string filePath = _mainDirectory;
            if(!string.IsNullOrEmpty(rootDir))
            {
                filePath = Path.Combine(filePath, rootDir);
                if(!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            filePath = Path.Combine(filePath, string.Format("{0}.xml", fileNameWithoutExtension));

            xDoc.Save(filePath, SaveOptions.None);
        }
    }
}
