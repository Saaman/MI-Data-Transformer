using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using System.Xml.Serialization;

namespace MIProgram.DataAccess
{
    public class XmlFileRepository : IFileRepository<IList<MIDBRecord>>
    {
        private readonly string _fileName;

        static int _errorsCount;
        // Validation Error Message
        static string _errorMessage = "";

        //CallBack appelée en cas d'erreur de validation du flux xml
        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            _errorMessage = _errorMessage + e.Message + "\r\n";
            _errorsCount++;
        }

        public XmlFileRepository(string fileName)
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
            var reader = new XmlTextReader(_fileName);//le lecteur du fichier
            var schemaSet = new XmlSchemaSet();//conteneur du schéma
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mi_reviews.xsd");
            if (!File.Exists(path))
            {
                message = string.Format("Le fichier source '{0}' n'existe pas", _fileName);
                return false;
            }
            schemaSet.Add(null, path);

            //paramètres de validation
            var settings = new XmlReaderSettings {ValidationType = ValidationType.Schema, Schemas = schemaSet};
            settings.ValidationEventHandler += ValidationCallBack;

            // Parse the file.  
            while (reader.Read())
            { }
            reader.Close();

            message = _errorMessage;

            return _errorsCount == 0;
        }

        public IList<MIDBRecord> GetData()
        {
            var serializer = new XmlSerializer(typeof(MIXmlRecord));
            IList<MIDBRecord> results = new List<MIDBRecord>();

            var xmlDoc = new XmlDocument();
            var input = File.ReadAllText(_fileName, Encoding.GetEncoding("ISO-8859-1"));
            input = input.Replace(string.Format("{0}", (char)133), "...");
            input = input.Replace(string.Format("{0}", (char)146), "'");
            input = input.Replace(string.Format("{0}", (char)10), "");
            input = input.Replace(string.Format("{0}", (char)171), "\"");
            input = input.Replace(string.Format("{0}", (char)187), "\"");

            Encoding xmlCod = Encoding.GetEncoding("ISO-8859-1");
            // Create two different encodings.
            Encoding winCod = Encoding.GetEncoding(1252);
            byte[] winBytes = winCod.GetBytes(input);
            // Perform the conversion from one encoding to the other.			 
            byte[] ebcdic = Encoding.Convert(xmlCod, winCod, winBytes);
            string output = winCod.GetString(ebcdic);


            xmlDoc.LoadXml(output);
            foreach (XmlNode node in xmlDoc.SelectNodes("/metalimpact/mi_reviews"))
            {
                using (var sr = new StringReader(node.OuterXml))
                {
                    results.Add(new MIDBRecord((MIXmlRecord)serializer.Deserialize(sr)));
                }
            }

            return results;
        }

        public void WriteData(IList<MIDBRecord> data)
        {
            throw new NotImplementedException();
        }
    }
}
