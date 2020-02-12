using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class AddProjectModelView : ObservableObject, IPageViewModel
    {
        ObservableCollection<string> _selectedUsers = new ObservableCollection<string>();
        ObservableCollection<string> _selectedFiles = new ObservableCollection<string>();
        public ICommand  Save{ get; set; }
        
        public AddProjectModelView()
        {
            TitrePage = "Ajouter un projet";
            LoadAction();
            LstUser = Singleton.GetInstance().GetAllUtilisateur().Where(user => user.Email != Singleton.GetInstance().GetUtilisateur().Email).Select(u => u.Login).ToList() ;
            LoadFiles();
        }

       #region [Binding]
        private string titrePage;
        public string TitrePage
        {
            get { return this.titrePage; }
            set
            {
                if (!string.Equals(this.titrePage, value))
                {
                    this.titrePage = value;
                    RaisePropertyChanged(nameof(TitrePage));
                }
            }
        }

        private string nomProjet;
        public string NomProjet
        {
            get { return this.nomProjet; }
            set
            {
                if (!string.Equals(this.nomProjet, value))
                {
                    this.nomProjet = value;
                    RaisePropertyChanged(nameof(NomProjet));
                }
            }
        }

        private string descriptionProjet;
        public string DescriptionProjet
        {
            get { return this.descriptionProjet; }
            set
            {
                if (!string.Equals(this.descriptionProjet, value))
                {
                    this.descriptionProjet = value;
                    RaisePropertyChanged(nameof(DescriptionProjet));
                }
            }
        }

        private List<string> _lstUser;
        public List<string> LstUser
        {
            get { return _lstUser; }
            set
            {
                if (!string.Equals(this._lstUser, value))
                {
                    this._lstUser = value;
                    RaisePropertyChanged(nameof(LstUser));
                }
            }
        }

        private List<string> _lstFiles;
        public List<string> LstFiles
        {
            get { return _lstFiles; }
            set
            {
                if (!string.Equals(this._lstFiles, value))
                {
                    this._lstFiles = value;
                    RaisePropertyChanged(nameof(LstFiles));
                }
            }
        }

        public ObservableCollection<string> SelectedUsers
        {
            get
            {
                return _selectedUsers;
            }
        }
        public ObservableCollection<string> SelectedFiles
        {
            get
            {
                return _selectedFiles;
            }
        }
        #endregion

        #region [Action]

        private void ActionAddProject(object parameter)
        {
            if (NomProjet != "" && DescriptionProjet != "")
            {
                Projet p = new Projet();
                p.NomProject = NomProjet;
                p.Description = DescriptionProjet;
                p.LstFiles = GetFilesProject();
                p.LstUser = GetUserProject();
                p.IcoIsArchived = "notvalidate.png";
                p.IsprojetFin = false;
                p.IsprojetEncours = true;
                p.DateDeCreation = DateTime.Now;
                Singleton.GetInstance().addProject(p);
                shareFile(p);
                Functions.CreateFileProjet();
                Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AdminModelView();
            }
            else
            {
                MessageBox.Show("Veuillez renseigner tous les champs obligatoires");
            }
        }

        //partage les fichiers dropbox avec les différents utilisateurs
        private void shareFile(Projet p)
        {
            foreach (Fichier f in p.LstFiles) {
                if (f.IdDropbox != null) {
                    foreach (Utilisateur u in p.LstUser) {
                        Singleton.GetInstance().GetHomeModelView().driveBaseDropbox.SharingFile(f, u);
                     }
                }
                else
                {
                    //non fait
                }
            }
        }

        #endregion

        /// <summary>
        /// Récupérer les fichiers des drive 
        /// </summary>
        private void LoadFiles()
        {
            List<string> lst_file_gg = Singleton.GetInstance().GetListModelView().DgFiles_GG.Select(f => f.Nom).ToList();
            List<string> lst_file_dp = Singleton.GetInstance().GetListModelView().DgFiles_DP.Select(f => f.Nom).ToList();
            List<string> rslt = new List<string>();
            rslt.AddRange(lst_file_dp);
            rslt.AddRange(lst_file_gg);
            LstFiles = rslt;
        }

        public void LoadAction()
        {
            Save = new RelayCommand(ActionAddProject);
        }

        private List<Utilisateur> GetUserProject()
        {
            List<Utilisateur> lst = new List<Utilisateur>();
            if (SelectedUsers != null)
            {
                List<Utilisateur> lstUtilisateur = Singleton.GetInstance().GetAllUtilisateur();

                foreach (var item in SelectedUsers)
                {
                    Utilisateur u = lstUtilisateur.FirstOrDefault(user => user.Login == item);
                    if (u != null)
                    {
                        lst.Add(u);
                    }
                }
            }
            return lst;
        }

        private List<Fichier> GetFilesProject()
        {
            List<Fichier> lst = new List<Fichier>();
            if (SelectedFiles != null)
            {
                List<Fichier> lstdp = Singleton.GetInstance().GetListModelView().DgFiles_DP;

                foreach (var item in SelectedFiles)
                {
                    Fichier u = lstdp.Find(f => f.Nom == item);
                    if (u != null)
                    {
                        lst.Add(u);
                    }
                }

                List<Fichier> lstgg = Singleton.GetInstance().GetListModelView().DgFiles_GG;

                foreach (var item in SelectedFiles)
                {
                    Fichier u = lstgg.Find(f => f.Nom == item);
                    if (u != null)
                    {
                        lst.Add(u);
                    }
                }
            }
            return lst;
        }
    }
}
