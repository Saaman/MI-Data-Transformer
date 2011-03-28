using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MIProgram.Core.Writers
{
    public interface IXmlWriter
    {
        void Write(XDocument xDoc, string fileNameWithoutExtension, string rootDir);
    }
}
