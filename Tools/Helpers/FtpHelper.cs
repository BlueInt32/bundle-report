using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Tools.Helpers
{
    public class FtpHelper
    {
        /// <summary>
        /// Envoie un fichier local sur un serveur ftp.
        /// </summary>
        /// <param name="localPath">Chemin complet du fichier local (filesystem uniquement)</param>
        /// <param name="fullDestinationPath">Chemin complet du fichier distant. Exemple: ftp://[ip]/chemin/distant/fichier.ext</param>
        /// <param name="ftpLogin">login ftp</param>
        /// <param name="ftpPass">pass ftp</param>
        public static void PushFileFTP(string localPath, string fullDestinationPath, string ftpLogin, string ftpPass)
        {
            // Get the object used to communicate with the server.
            //string distantPath = string.Format("ftp://{0}{1}",
            //                                    ConfigurationManager.AppSettings["ftpServer"],
            //                                    ConfigurationManager.AppSettings["ftpFilePath"].Replace("#Date#", DateTime.Now.ToString("yyyyMMdd")));
            string distantPath = fullDestinationPath;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(distantPath);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(
                //ConfigurationManager.AppSettings["ftpLogin"], 
                //ConfigurationManager.AppSettings["ftpPass"]
                ftpLogin,
                ftpPass
               );

            // Copy the contents of the file to the request stream.
            StreamReader sourceStream = new StreamReader(localPath);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            //Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
        }
    }
}
