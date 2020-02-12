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
    public class UpdateProjectModelView : ObservableObject, IPageViewModel
    {
        ObservableCollection<string> _selectedUsers = new ObservableCollection<string>();
        ObservableCollection<string> _selectedFiles = new ObservableCollection<string>();
        private  Projet Projet { get; set; }
        public ICommand Save { get; set; }

        public UpdateProjectModelView(Projet project)
        {
            try
            {
                Projet = Singleton.GetInstance().GetAllProject().FirstOrDefault(p => p.NomProject.Equals(project.NomProject));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :\" " + ex.Message);
            }

            TitrePage = "Modifier un projet";
            LoadInformation();
            LoadAction();
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

        private void LoadInformation()
        {
            NomProjet = Projet.NomProject;
            DescriptionProjet = Projet.Description;
            List<Utilisateur> lstUtilisateur = Singleton.GetInstance().GetAllUtilisateur();
            LstUser = Projet.LstUser.Select(u => u.Login).ToList();
            LstFiles = Projet.LstFiles.Select(f => f.Nom).ToList();
        }

        public void LoadAction()
        {
            Save = new RelayCommand(ActionUpdateProject);
        }

        private void ActionUpdateProject(object obj)
        {
            List<Projet> lst = Singleton.GetInstance().GetAllProject();
            Projet = lst.FirstOrDefault(item => item.NomProject.Equals(Projet.NomProject));

            Projet.NomProject = nomProjet;
            Projet.Description = DescriptionProjet;
            Functions.CreateFileProjet();
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AdminModelView();
            
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
    }
}
