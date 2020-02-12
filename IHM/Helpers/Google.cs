
using Com.CloudRail.SI;
using Com.CloudRail.SI.Interfaces;
using Com.CloudRail.SI.ServiceCode.Commands.CodeRedirect;
using Com.CloudRail.SI.Services;
using Com.CloudRail.SI.Types;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Download;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using File = Google.Apis.Drive.v3.Data.File;


namespace IHM.Helpers
{
    public class GoogleCloud : DriveBase
    {
        ICloudStorage serviceCloudStorage; 
        private Utilisateur u = Singleton.GetInstance().GetUtilisateur();

        public GoogleCloud()  {

            CloudRail.AppKey = Constant.LicenceCloudRail;
            GoogleDrive googledrive =  new GoogleDrive(new LocalReceiver(8082), Constant.GoogleKey, Constant.GooGleSecret,"http://localhost:8082/auth", "someState");
            serviceCloudStorage = googledrive;
        }

        public override string Connect()
        {
            serviceCloudStorage.Login();
            String result = serviceCloudStorage.SaveAsString();
            serviceCloudStorage.LoadAsString(result);
            return result;
        }

        public override string GetCompteClient()
        {
            if (serviceCloudStorage == null)
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);

            return serviceCloudStorage.GetUserLogin();
        }

        public override List<Fichier> GetItems()
        {
            List<Fichier> FileList = new List<Fichier>();
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            List<CloudMetaData> result = serviceCloudStorage.GetChildren("/");

            foreach (CloudMetaData c in result)
            {
                Fichier File = new Fichier();
                File.Nom = c.GetName();
                File.Taille = c.GetSize().ToString();
                File.path = c.GetPath();
                FileList.Add(File);
            }

            return FileList.OrderBy(f => f.Nom).ToList(); 
        }
        
        public override List<Fichier> GetItemsFolder(string folderPath)
        {
            List<Fichier> FileList = new List<Fichier>();
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            List<CloudMetaData> result = serviceCloudStorage.GetChildren(folderPath);

            foreach (CloudMetaData c in result)
            {
                Fichier File = new Fichier();
                File.Nom = c.GetName();
                File.Taille = c.GetSize().ToString();
                File.path = c.GetPath();
                FileList.Add(File);
            }

            return FileList.OrderBy(f => f.Nom).ToList(); ;
        }
        
        public override bool CreateFolder( string path, string nameFolder)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
                serviceCloudStorage.CreateFolder(path + nameFolder);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public override bool Delete( string path)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
                serviceCloudStorage.Delete(path);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public override bool Download(string pathGoogle, string pathLocal)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            Stream stream = serviceCloudStorage.Download(pathGoogle);


            using (FileStream fileStream = System.IO.File.Create(pathLocal, (int)stream.Length))
            {
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }

            return true;
        }

        public override bool FolderExists(string path)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            return serviceCloudStorage.Exists("/myFolder");
        }

        public override List<Fichier> Search(string str)
        {
            List<Fichier> FileList = new List<Fichier>();
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            List<CloudMetaData> result = serviceCloudStorage.Search( str);
            foreach (CloudMetaData c in result)
            {
                Fichier File = new Fichier();
                File.Nom = c.GetName();
                File.Taille = c.GetSize().ToString();
                File.path = c.GetPath();
                FileList.Add(File);
            }

            return FileList.OrderBy(f => f.Nom).ToList(); ;
        }

        public override bool Upload(string path)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
                var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
                serviceCloudStorage.Upload( path, stream, 1024, true);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public override string CreateShareLink( string filepath)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            return serviceCloudStorage.CreateShareLink(filepath);
        }

        public override bool Watch(string nom, string fileId)
        {
            throw new NotImplementedException();
        }

        public override void GetCompteClient(string token_dp, string _accesstoken, string refreshtoken)
        {
            throw new NotImplementedException();
        }
        
        public override List<Fichier> GetFilesShared()
        {
            throw new NotImplementedException();
        }

        public override bool getSpace()
        {
            throw new NotImplementedException();
        }

        public override bool SharingFile(Fichier fichier, Utilisateur utilisateur)
        {
            throw new NotImplementedException();
        }
                
    }
}
