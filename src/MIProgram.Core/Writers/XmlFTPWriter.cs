using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Xml;

namespace MIProgram.Core.Writers
{
    public class XmlFTPWriter : IXmlWriter
    {
        private string _ftpServer;
        private string _ftpUser;
        private string _ftpPassword;

        public XmlFTPWriter(string ftpServer, string ftpUser, string ftpPassword)
        {
            if (string.IsNullOrEmpty(ftpServer))
                throw new ArgumentNullException("ftpServer");
            if (string.IsNullOrEmpty(ftpUser))
                throw new ArgumentNullException("ftpUser");
            if (string.IsNullOrEmpty(ftpPassword))
                throw new ArgumentNullException("ftpPassword");

            _ftpServer = ftpServer;
            _ftpPassword = ftpPassword;
            _ftpUser = ftpUser;
        }

        public void Write(XDocument xDoc, string fileNameWithoutExtension, string rootDir)
        {
            string uri = "ftp://" + _ftpServer + "/";
            if (!string.IsNullOrEmpty(rootDir))
            {
                uri = uri + rootDir + "/";
            }

            uri = uri + string.Format("{0}.xml", fileNameWithoutExtension);

            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            // Provide the WebPermission Credintials
            reqFTP.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);

            // By default KeepAlive is true, where the control connection is not closed after a command is executed.
            reqFTP.KeepAlive = false;

            // Specify the command to be executed.
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.
            reqFTP.UseBinary = false;

            // Stream to which the file to be upload is written
            using (Stream strm = reqFTP.GetRequestStream())
            {
                using (var writer = XmlWriter.Create(strm, new XmlWriterSettings(){Indent=true}))
                {
                    xDoc.WriteTo(writer);
                }
            }
        }
    }
}