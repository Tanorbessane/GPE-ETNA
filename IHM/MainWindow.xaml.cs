using IHM.Helpers;
using IHM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows;

namespace IHM
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateFileJSON();
        }

        /// <summary>
        /// Creation de fichier JSON dans le dossier temp de l'utilisateur
        /// </summary>
        private void CreateFileJSON()
        {
            string path_projet = Constant.path_projet; 
            string path_role = Constant.path_role; 
            string path_utilisateur = Constant.path_utilisateur;

            try
            {
                if (!File.Exists(path_projet))
                {
                    using (StreamWriter file = File.CreateText(@path_projet))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, new List<Projet>());
                    }
                }

                if (File.Exists(path_role))
                {
                    File.Delete(path_role);
                }

                    List<Fonctionnalites> fonctionnalite_secretaire = new List<Fonctionnalites>();
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lister les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Downloader les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Supprimer les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Ajouter un dossier de dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Visualiser les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Renommer un fichier d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les roles des utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Lier un fichier à un projet" });


                    List<Fonctionnalites> fonctionnalite_chef = new List<Fonctionnalites>();
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lister les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Downloader les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Supprimer les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Ajouter un dossier de dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Visualiser les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Renommer un fichier d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les projets de l'application" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les roles des utilisateurs de l'application" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lier un fichier à un projet" });

                    List<Fonctionnalites> fonctionnalite_gestionnaire = new List<Fonctionnalites>();
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lister les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Downloader les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Supprimer les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Ajouter un dossier de dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Visualiser les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Renommer un fichier d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Gerer les projets de l'application" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Gerer les roles des utilisateurs de l'application" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Lier un fichier à un projet" });

                    List<Roles> lst_role = new List<Roles>();
                    lst_role.Add(  new Roles { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Secrétaire" , lstFontionnalites = fonctionnalite_secretaire } );
                    lst_role.Add(new Roles { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Développeur", lstFontionnalites = fonctionnalite_secretaire });
                    lst_role.Add(new Roles() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Chef de projet" , lstFontionnalites = fonctionnalite_chef});
                    lst_role.Add(new Roles() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gestionnaire de cloud", lstFontionnalites = fonctionnalite_gestionnaire });


                    using (StreamWriter file = File.CreateText(path_role))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, lst_role);
                    }
                

                if (!File.Exists(path_utilisateur))
                {
                    using (StreamWriter file = File.CreateText(path_utilisateur))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, new List<Utilisateur>());
                    }
                }

                if (!File.Exists(Constant.ClientSecretJSON))
                {
                    string str = "{'installed':{'client_id':'654071449191-danfhp839mq0ivi7a0ddkm4oo0g9o5ju.apps.googleusercontent.com','project_id':'focal-appliance-204212','auth_uri':'https://accounts.google.com/o/oauth2/auth','Token_DP_uri':'https://accounts.google.com/o/oauth2/Token_DP','auth_provider_x509_cert_url':'https://www.googleapis.com/oauth2/v1/certs','client_secret':'JZ_Pi9D4--QS2jZ8SfnTxBWW','redirect_uris':['urn:ietf:wg:oauth:2.0:oob','http://localhost']}}";
                    JavaScriptSerializer j = new JavaScriptSerializer();
                    object a = j.Deserialize(str, typeof(object));

                    using (StreamWriter file = File.CreateText(Constant.ClientSecretJSON))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, a);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
