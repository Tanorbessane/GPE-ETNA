using GPE;
using IHM.Model;
using IHM.ModelView;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IHM.Helpers
{
    class Singleton
    {
        /**Cryptage**/
        //static TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        static RijndaelManaged provider = new RijndaelManaged();

        List<Utilisateur> lstUtilisateur = new List<Utilisateur>();
        List<Projet> lstProject = new List<Projet>();
        List<Roles> lstRoles = new List<Roles>();
        DropBox DBB = new DropBox(ConfigurationSettings.AppSettings["strAppKey"], "PTM_Centralized");
        GoogleCloud google  = new GoogleCloud();
        Cloud cloud;
        MainModelView cMain;
        HomeModelView cContent;
        Utilisateur cUtilisateur;
        RolesModelView rolesModelView;
        ListModelView cList;

        static Singleton _instance;

        public static Singleton GetInstance()
        {
            if (_instance == null)
                _instance = new Singleton();
            return _instance;
        }

        /********/

        public void SetMainWindowViewModel(MainModelView mainWindowViewModel)
        {
            cMain = mainWindowViewModel;
        }
        public MainModelView GetMainWindowViewModel()
        {
            return cMain;
        }

        public void setListFilesView(ListModelView listModelView)
        {
            cList = listModelView;
        }

        public ListModelView GetListModelView()
        {
            return cList;
        }

        /********/

        public void SetHomeModelView(HomeModelView homeModelView)
        {
            cContent = homeModelView;
        }
        public HomeModelView GetHomeModelView()
        {
            return cContent;
        }

        /********/

        public void SetUtilisateur(Utilisateur u)
        {
            cUtilisateur = u;
        }
        public Utilisateur GetUtilisateur()
        {
            return cUtilisateur;
        }
      
        /********/

        public void SetRolesModelView(RolesModelView _rolesModelView)
        {
            rolesModelView = _rolesModelView;
        }
        public RolesModelView GetRolesModelView()
        {
            return rolesModelView;
        }
        /********/

        public void addUtilisateur(Utilisateur u)
        {
            lstUtilisateur.Add(u);
        }
        public List<Utilisateur> GetAllUtilisateur()
        {
            return lstUtilisateur;
        }

        public void SetListUtilisateur(List<Utilisateur> lstu)
        {
            lstUtilisateur.Clear();
            lstUtilisateur = lstu;
        }

        /********/

        public void SetDBB(DropBox _DBB)
        {
            DBB = _DBB;
        }
        public DropBox GetDBB()
        {
            return DBB;
        }

        /************/

        public void SetCloud(Cloud _cloud)
        {
            cloud = _cloud;
        }
        public Cloud GetCloud()
        {
            return cloud;
        }

        /************/
        public void SetGoogle(GoogleCloud _google)
        {
            google = _google;
        }
        public GoogleCloud GetGoogle()
        {
            return google;
        }

        /********/

        public void addProject(Projet p)
        {
            lstProject.Add(p);
        }

        public Projet getProjetByName(string name)
        {
            return lstProject.First(p => p.NomProject.Equals(name));
        }


        public List<Projet> getProjetByFileId(Fichier fichier)
        {
            List<Projet> result = new List<Projet>();

            lstProject.ForEach(p =>
            {
                foreach(Fichier f in p.LstFiles)
                {
                    if (fichier.IdDropbox == f.IdDropbox || fichier.IdGoogle == f.IdGoogle)
                    {
                        result.Add(p);
                        break;
                    }
                }
            });
            return result;
        }

        internal void GetDeleteProject(Projet SelectedProject)
        {
            Projet p = lstProject.Find(s => s.NomProject.Equals(SelectedProject.NomProject));
            lstProject.Remove(p);
            Functions.CreateFileProjet();
        }

        public List<Projet> GetAllProject()
        {
            return lstProject;
        }

        public void UpdateProject(Projet pr)
        {
            var index = lstProject.FindIndex(p => p.NomProject.Equals(pr.NomProject));
            lstProject[index] = pr;
            Functions.CreateFileProjet();
        }

        public void SetListProject(List<Projet> _lstProject)
        {
            lstProject.Clear();
            lstProject = _lstProject;
        }

        /********/

        public void addRole(Roles p)
        {
            lstRoles.Add(p);
        }
        public List<Roles> GetAllRole()
        {
            return lstRoles;
        }

        public void SetListRole(List<Roles> _lstRole)
        {
            lstRoles.Clear();
            lstRoles = _lstRole;
        }

        public Roles GetRoleByNom(string nom)
        {
            if (lstRoles != null && lstRoles.Count > 0)
            {
                return lstRoles.FirstOrDefault(x => x.Nom.Equals(nom));
            }
            else
            {
                return null;
            }
        }

        public byte[] Encrypt(string btext)
        {
            using (MemoryStream mstream = new MemoryStream())
            {
                using (CryptoStream cstream = new CryptoStream(mstream, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cstream.Write(Encoding.ASCII.GetBytes(btext), 0, btext.Length);
                }
                return mstream.ToArray();
            }
        }

        public string Decrypt(byte[] btext)
        {
            using (MemoryStream mstream = new MemoryStream(btext))
            {
                using (CryptoStream cstream = new CryptoStream(mstream, provider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] output = new byte[cstream.Length];
                    cstream.Read(output, 0, output.Length);
                    return output.ToString();
                }
            }
        }

    }
}

