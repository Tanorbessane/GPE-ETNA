using Dropbox.Api;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Helpers
{
    public abstract class DriveBase
    {

        //Dropbox
        public DropboxClient DBClient;
        public long espace_utilise;
        private string oauth2State;
        private const string RedirectUri = "https://localhost/authorize";

        #region [Constructeur]
        public DriveBase() { }
        public DriveBase(string ApiKey, string ApiSecret, string ApplicationName = "TestApp") { }
        #endregion

        public abstract string Connect();
        public abstract bool CreateFolder(string path, string nameFolder);
        public abstract bool Delete(string path);
        public abstract string GetCompteClient();
        public abstract bool Download(string pathGoogle, string pathLocal);
        public abstract List<Fichier> GetItemsFolder(string folderPath);
        public abstract List<Fichier> GetItems();
        public abstract bool FolderExists(string path);
        public abstract List<Fichier> Search(string str);
        public abstract bool Upload(string path);
        public abstract string CreateShareLink(string filepath);


        //**//
        public abstract void GetCompteClient(string token_dp, string _accesstoken, string refreshtoken);
        public abstract List<Fichier> GetFilesShared();
        public abstract bool getSpace();
        public abstract bool SharingFile( Fichier fichier, Utilisateur utilisateur);
        public abstract bool Watch( string nom, string fileId);

    }
}
