using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Xml;

namespace MIProgram.Core.DAL.Writers
{
    public class FTPFileWriter : IWriter
    {
        private readonly string _ftpServer;
        private readonly string _ftpUser;
        private readonly string _ftpPassword;

        public FTPFileWriter(string ftpServer, string ftpUser, string ftpPassword)
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

        public void WriteXML(XDocument xDoc, string fileNameWithoutExtension, string rootDir)
        {
            string uri = "ftp://" + _ftpServer + "/";
            if (!string.IsNullOrEmpty(rootDir))
            {
                uri = uri + rootDir + "/";
            }

            uri = uri + string.Format("{0}.xml", fileNameWithoutExtension);

            // Create FtpWebRequest object from the Uri provided
            var reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uri));

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
                using (var writer = XmlWriter.Create(strm, new XmlWriterSettings {Indent=true}))
                {
                    xDoc.WriteTo(writer);
                }
            }
        }

        public void WriteTextCollection(IList<string> collection, string fileNameWithoutExtension, string rootDir)
        {
            throw new NotImplementedException("You are not allowed to publish such information on FTP");
        }

        public void WriteCSV<T>(IList<T> collection, string fileNameWithoutExtension, string rootDir)
        {
            throw new NotImplementedException("You are not allowed to publish such information on FTP");
        }

        public void WriteSQL(string sqlString, string fileNameWithoutExtension, string rootDir)
        {
            throw new NotImplementedException("You are not allowed to publish such information on FTP");
        }

        public void WriteRB(string railsString, string fileNameWithoutExtension, string rootDir)
        {
            throw new NotImplementedException("You are not allowed to publish such information on FTP");
        }
    }
}