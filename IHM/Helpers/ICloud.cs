using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3.Data;
using GPE;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Helpers
{
    /// <summary>
    /// Enumération des types de clouds utilisés dans l'application
    /// </summary>
    public enum Drive {
        DP = 0, GG = 1
    }

    /// <summary>
    /// Classe permettant de selectionner la bonne fonction en fonction du cloud
    /// </summary>
    public class Cloud : ICloud
    {
        private DropBox DBB = Singleton.GetInstance().GetDBB();
        private GoogleCloud Google = Singleton.GetInstance().GetGoogle();


        public void GetCompteClient(string token_dp, string _accesstoken, string refreshtoken)
        {
            if (token_dp != "" && token_dp != null)
                DBB.GetDBClient(token_dp);

            if (_accesstoken != null && refreshtoken != null)
            {
                UserCredential rsltCredential = Google.CreateCredential(new TokenResponse { AccessToken = _accesstoken, RefreshToken = refreshtoken });
                Google.GetGoogleService(rsltCredential);
            }
        }


        /// <summary>
        /// Creer un dossier sur un cloud
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CreateFolder(Drive drive, string path, string nameFolder)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        DBB.CreateFolder(path);
                        break;
                    case Drive.GG:
                        Google.CreateFolder(nameFolder);
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Supprime un fichier en fonction de son Id ou son chemin
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Delete(Drive drive, string path, string fileId)
        {
            bool result = false;
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        DBB.Delete(path);
                        break;
                    case Drive.GG:
                        Google.Delete(fileId);
                        break;
                }
                result = true;
            }
            catch (Exception)
            {
                result =  false;
            }
            return result;
        }

        /// <summary>
        /// Télécharge un fichier correspondant à un cloud.
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="FolderPath"></param>
        /// <param name="FileName"></param>
        /// <param name="DownloadFolderPath"></param>
        /// <param name="DownloadFileName"></param>
        /// <returns></returns>
        public bool Download(Drive drive, string FolderPath, string FileName, string DownloadFolderPath, string DownloadFileName, string fileId, string mimeType)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        DBB.Download(FolderPath, FileName, DownloadFolderPath, DownloadFileName);
                        break;
                    case Drive.GG:
                      Google.Download(FileName,  fileId, DownloadFolderPath);
                        break;
                }
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie si un dossier existe avant de le creer
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FolderExists(Drive drive, string path, string nameFolder)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        DBB.FolderExists(path);
                        break;
                    case Drive.GG:
                        Google.FolderExists(nameFolder);
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Récupère les fichiers en consultation
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public List<Fichier> GetFilesShared(Drive drive)
        {
            List<Fichier> list = new List<Fichier>();
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        DBB.GetFilesShared();
                        break;
                    case Drive.GG:
                        Google.GetFilesShared();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// Récupère la listes des fichiers correspondant à un cloud
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public List<Fichier> GetItems(Drive drive)
        {
            List<Fichier> lst = new List<Fichier>();

            try {
                switch (drive)
                {
                    case Drive.DP:
                        if (DBB != null)
                            lst = DBB.GetItems();
                            lst.AddRange(DBB.GetFilesShared());
                        break;
                    case Drive.GG:
                        if (Google != null)
                            lst = Google.GetItems();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }

        /// <summary>
        /// Récupèr l'espace disponible sur le drive
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public bool getSpace(Drive drive)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Partage un fichier avec un utilisateur
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="fichier"></param>
        /// <param name="utilisateur"></param>
        /// <returns></returns>
        public bool SharingFile(Drive drive, Fichier fichier, Utilisateur utilisateur)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload un fichier sur un drive
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="UploadfolderPath"></param>
        /// <param name="UploadfileName"></param>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public bool Upload(Drive drive, string UploadfolderPath, string UploadfileName, string SourceFilePath, string type)
        {
            bool result = false;
            try
            {
                if (drive == Drive.DP)
                {
                    result = DBB.Upload(UploadfolderPath, UploadfileName, SourceFilePath);
                }
                else
                {
                    result = Google.Upload(UploadfileName, type, SourceFilePath);
                }

               
            }
            catch (Exception ex)
            {
               //
            }
            return result;
        }

        /// <summary>
        /// Visualiser un fichier
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public bool Watch(Drive drive, string nom ,string fileId)
        {
            bool result = false;
            try
            {
                if(drive == Drive.GG)
                     result = Google.Watch(nom, fileId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }

    public interface ICloud
    {
        List<Fichier> GetItems(Drive drive);
        bool Download(Drive drive, string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName, string fileId, string mimeType);
        bool CreateFolder(Drive drive, string path, string nameFolder);
        bool FolderExists(Drive drive, string path, string nameFolder);
        bool Delete(Drive drive, string path, string fileId);
        bool Upload(Drive drive, string UploadfolderPath, string UploadfileName, string SourceFilePath, string type);
        bool SharingFile(Drive drive, Fichier fichier, Utilisateur utilisateur);
        List<Fichier> GetFilesShared(Drive drive);
        bool getSpace(Drive drive);
    }
}