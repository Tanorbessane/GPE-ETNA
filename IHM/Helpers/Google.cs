using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v3.Data;
using IHM.Model;
using System.Configuration;
using Newtonsoft.Json;
using System.Windows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using IHM.ModelView;
using Google.Apis.Download;
using File = Google.Apis.Drive.v3.Data.File;

namespace IHM.Helpers
{
    public class GoogleCloud
    {
        public static string[] Scopes = { DriveService.Scope.Drive };
        public static DriveService service = null;
        static Dictionary<string, Google.Apis.Drive.v3.Data.File> files = new Dictionary<string, Google.Apis.Drive.v3.Data.File>();
        private string clientJson = Constant.ClientSecretJSON; 

        public GoogleCloud()  { }

        /// <summary>
        /// Réupération de l'autorisation de l'utilisateur
        /// </summary>
        /// <returns></returns>
        public UserCredential GetAuthorization()
        {
            UserCredential credential = null;
            service = null;
            try
            {
                if (Directory.Exists(clientJson) == true)
                    Directory.Delete(clientJson, true);

                using (var stream = new FileStream(clientJson, FileMode.Open, FileAccess.Read))
                {                    
                    String FilePath = Constant.strAppSecretGoogle;
                   
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(FilePath, true)).Result;
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            return credential;            
        }
        
        /// <summary>
        /// Enregistrement du token
        /// </summary>
        /// <param name="credential"></param>
        public void SaveToken(UserCredential credential)
        {
            //Récupération du token 
            var token = new TokenResponse
            {
                AccessToken = credential.Token.AccessToken,
                RefreshToken = credential.Token.RefreshToken
            };
            
            List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
            Utilisateur _u = lst.FirstOrDefault(item => item.Login.Equals(Singleton.GetInstance().GetUtilisateur().Login));
            _u.Token_GG = token.AccessToken;
            _u.RefreshToken = token.RefreshToken;
            Functions.CreateFileUtilisateur();
        }

        /// <summary>
        /// A faire verification avant l'ajout
        /// </summary>
        /// <param name="nameFolder"></param>
        public void FolderExists(string nameFolder)
        {
            //a faire
        }

        /// <summary>
        /// Creation d'un credential à partir d'un token
        /// </summary>
        public UserCredential CreateCredential(TokenResponse token)
        {
            ClientSecrets clientsecret;
            
            //récupération du client secret
            using (var stream = new FileStream(clientJson, FileMode.Open, FileAccess.Read))
            {
                clientsecret = GoogleClientSecrets.Load(stream).Secrets;
            }

            //recuperation du code d'authorization
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientsecret,
                Scopes = Scopes,
                DataStore = new FileDataStore("Store")
            });

            var googleServiceCredential = new UserCredential(flow, Environment.UserName, token);
            return googleServiceCredential;
        }

        public void GetGoogleService(UserCredential credential)
        {
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveRestAPI-v3",
            });
        }

        /// <summary>
        /// Connection de Google Drive
        /// </summary>
        public void Connect()
        {
            UserCredential uc =  GetAuthorization();
            SaveToken(uc);
            GetGoogleService(uc);
            GetRootFolderId();
            GetItems();
        }

        public string GetRootFolderId()
        {
            var rootFolder = service.Files.Get("root").Execute();
            return rootFolder.Id;
        }
        
        /// <summary>
        /// Récupère les fichiers de google drive
        /// </summary>
        /// <returns></returns>
        public List<Fichier> GetItems()
        {
            FilesResource.ListRequest FileListRequest = service.Files.List();
            FileListRequest.Fields = "nextPageToken, files(id, name, size, version, createdTime, parents, fileExtension, iconLink, owners)";
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
            List<Fichier> FileList = new List<Fichier>();
            
            if (files != null && files.Count > 0)
            {                
                List<Fichier> filesSansDossier = new List<Fichier>();
                string root = GetRootFolderId();
                foreach (var f in files)
                {
                    if (f.Parents != null)
                    {
                        foreach(string str_parent in f.Parents)
                        {
                            if (str_parent == root || str_parent == null)
                            {
                                Google.Apis.Drive.v3.Data.File cFile = GetFile(f.Id);
                                Fichier File = new Fichier();
                                File.IdGoogle = cFile.Id;
                                File.Nom = cFile.Name;
                                File.Taille = (cFile.Size == null ? "-" : cFile.Size.ToString());
                                File.Version = cFile.Version;
                                File.MimeType = cFile.MimeType;
                                File.DateDeCreation = f.CreatedTime;
                                File.IsFile = (cFile.Parents == null ? true : false);
                                File.PreviewUrl = (cFile.WebContentLink == null ? "" : cFile.WebContentLink);
                                File.IMG = cFile.IconLink == null ? cFile.IconLink : "";  
                                File.Type = (File.IsFile != false ? Path.GetExtension(cFile.Name) : "-");
                                FileList.Add(File);
                                break;
                            }
                        }
                    }
                }
            }
            return FileList.OrderBy(f => f.Nom).ToList();
        }

        /// <summary>
        /// Récupére les id des dossiers parents d'un fichier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetParentByFileId(string id)
        {
            string previousParents = "";
            try
            {
                var getRequest = service.Files.Get(id);
                getRequest.Fields = "parents";
                var test = getRequest.Execute();

                var test2 = service.About.Get().ExecuteAsStream();
                

                if (test.Parents != null)
                    previousParents = String.Join(",", test.Parents);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return previousParents;
        }

        /// <summary>
        /// Récupére les informations d'un fichier par son ID
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        private Google.Apis.Drive.v3.Data.File GetFile(String fileId)
        {
            Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
            try
            {
                file = service.Files.Get(fileId).Execute();
            }
            catch (Exception e)
            {

                Console.WriteLine("An error occurred: " + e.Message);
            }
            return file;
        }

        /// <summary>
        /// Télécharge un fichier dans un dossier cible
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileId"></param>
        /// <param name="DownloadFolderPath"></param>
        internal void Download (string filename, string fileId, string DownloadFolderPath)
        {

            MemoryStream stream1 = new MemoryStream();
            var request = service.Files.Get(fileId);

            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            SaveStream(stream1, DownloadFolderPath);
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream1);
            //return FilePath;
        }

        private static void SaveStream(MemoryStream stream, string DownloadFolderPath)
        {
            using (System.IO.FileStream file = new FileStream(DownloadFolderPath, FileMode.Create, FileAccess.ReadWrite))
            {
                stream.WriteTo(file);
            }
        }

        /// <summary>
       /// List MimeType
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
        private static string GetMimeType(string type)
        {
            string result = string.Empty;
            switch (type)
            {
                case "jpg":
                case "jpeg":
                    result = "image/jpeg";
                    break;
                case "png":
                    result = "image/png";
                    break;
                case "gif":
                    result = "image/gif";
                    break;
                case "pdf":
                    result = "application/pdf";
                    break;
                case "xls":
                    result = "application/vnd.ms-excel";
                    break;
                case "html":
                    result = "text/html";
                    break;
                case "txt":
                    result = "text/plain";
                    break;
                case "doc":
                    result = "application/msword";
                        break;
                case "docx":
                    result = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "csv":
                    result = "text/csv";
                    break;
                case "excel":
                    result = "application/vnd.ms-excel";
                    break;
                case "ppt":
                    result = "application/vnd.ms-powerpoint";
                    break;
                case "pptx":
                    result = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Importe un document dans google drive
        /// </summary>
        /// <param name="_uploadFile">Nom du fichier</param>
        /// <param name="type">Type du fichier </param>
        /// <param name="pathLocal"> chemin local du fichier</param>
        /// <returns></returns>
        public bool Upload(string _uploadFile, string type, string pathLocal)
        {
            try
            {
                var fileMetadata = new File()
                {
                    Name = _uploadFile,
                    MimeType = GetMimeTupeGoogle(type.Split('.')[1]),
                    CreatedTime = DateTime.Now                    
                };
                FilesResource.CreateMediaUpload request;
                using (var stream = new System.IO.FileStream(pathLocal, System.IO.FileMode.Open))
                {
                    request = service.Files.Create(fileMetadata, stream, GetMimeType(type.Split('.')[1]));
                    request.Fields = "name, id";
                    request.Upload();
                }
                var file = request.ResponseBody;
                return true;

            }catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Récupère le mimetype de google par rapport à l'extension du fichier
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetMimeTupeGoogle(string type)
        {
            string str = string.Empty;
            switch (type)
            {
                case "jpg":
                case "jpeg":
                case "png":
                case "gif":
                case "pdf":
                case "html":
                case "txt":
                case "doc":
                case "docx":
                    str = "application/vnd.google-apps.document";
                    break;
                case "csv":
                case "excel":
                case "tsv":
                    str = "application/vnd.google-apps.spreadsheet";
                    break;
                case "ppt":
                case "pptx":
                    str = "application/vnd.google-apps.presentation";
                    break;
            }
            return str;
        }

        /// <summary>
        /// Créé un fichier
        /// </summary>
        public void CreateFolder(string nameFolder)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = nameFolder,
                    MimeType = "application/vnd.google-apps.folder"
                };
                var request = service.Files.Create(fileMetadata);
                request.Fields = "id";
                var file = request.Execute();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Supprime un fichier dans Google Drive
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileId"></param>
        /// <param name="optional"></param>
        public void Delete(string fileId)
        {
            try
            {
                Verification(fileId);
                var request = service.Files.Delete(fileId);
                request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la suppression du fichier google.", ex);
            }
        }

        /// <summary>
        /// Vérifie que l'on possède un service et un id de fichier
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileId"></param>
        private static void Verification(string fileId)
        {
            if (service == null)
                throw new ArgumentNullException("Erreur de Service Google");
            if (fileId == null)
                throw new ArgumentNullException("Erreur de suppression du fichier google =>" + fileId);
        }

        /// <summary>
        /// Affichage d'un fichier depuis google drive
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public bool Watch(string filename, string fileId)
        {
            try
            {
                Verification(fileId);
                MemoryStream stream1 = new MemoryStream();
                var request = service.Files.Get(fileId);
                request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                string path = @"C:\Temp\" + filename;
                                SaveStream(stream1, path);
                                System.Diagnostics.Process.Start(path);
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
                request.Download(stream1);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la visualisation d'un document google.", ex);
                return false;
            }
        }

        /// <summary>
        /// Copie un fichier dans google drive.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileId"></param>
        /// <param name="body"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public Google.Apis.Drive.v3.Data.File Copy(DriveService service, string fileId, Google.Apis.Drive.v3.Data.File body)
        {
            try
            {
                Verification(fileId);

                if (body == null)
                    throw new ArgumentNullException("body");

                var request = service.Files.Copy(body, fileId);

                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la copie d'un fichier", ex);
            }
        }

        /// <summary>
        /// Recuperation des fichiers partagés avec l'utilisateur connecté.
        /// </summary>
        public void GetFilesShared()
        {
            List<Fichier> lstFiles = new List<Fichier>();
            try
            {
                //var request = service.;
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la copie d'un fichier", ex);
            }
            //return lstFiles;
        }
    }
}
