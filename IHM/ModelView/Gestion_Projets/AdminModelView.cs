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
    public class AdminModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Adminstration GED";
       
        public ICommand AddProject { get; set; }
        public ICommand ModifierProjet { get; set; }
        public ICommand SupprimerProjet { get; set; }

        #region [Constructor]
        public AdminModelView()
        {           
            LoadProject();
            LoadAction();
        }

        private void LoadProject()
        {
            if (LstProject != null)
                LstProject.Clear();

             List<Projet> lstProject = Singleton.GetInstance().GetAllProject();

            if (Singleton.GetInstance().GetUtilisateur().Role != "Chef de projet")
            {
                List<Projet> lstTmp = new List<Projet>();
                lstProject.ForEach(p =>
                {
                    p.LstUser.ForEach(u =>
                    {
                        if (u.Login.Equals(Singleton.GetInstance().GetUtilisateur().Login))
                        {
                            lstTmp.Add(p);
                        }
                    });
                });
                lstProject = lstTmp;
            }

            foreach (Projet p in lstProject)
            {
                string img = p.IcoIsArchived;
                p.IcoArchived = "/IMG/" + img;
                if (img != "/IMG/validate.png")
                {
                    p.IcoToolTip = "Projet fini";
                    p.IsprojetEncours = true;
                    p.IsprojetFin = false;
                }
                else
                {
                    p.IcoToolTip = "Projet en cours";
                    p.IsprojetEncours = false;
                    p.IsprojetFin = true;
                }
                p.RbEncours = "/IMG/notvalidate.png";
                p.RbFini = "/IMG/validate.png";
            }

            LstProject = lstProject;
        }

        #endregion

        #region [Binding]        
        private List<Projet> lstProject;
        public List<Projet> LstProject
        {
            get { return this.lstProject; }
            set
            {
                if (!string.Equals(this.lstProject, value))
                {
                    this.lstProject = value;
                    RaisePropertyChanged(nameof(LstProject));
                }
            }
        }

        private Projet selectedProject;
        public Projet SelectedProject
        {
            get { return this.selectedProject; }
            set
            {
                if (!string.Equals(this.selectedProject, value))
                {
                    this.selectedProject = value;
                    RaisePropertyChanged(nameof(SelectedProject));
                }
            }
        }
        #endregion

        #region [Action]
        private void ActionAddProject(object parameter)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AddProjectModelView();
        }

        private void ActionModifierProject(object parameter)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new UpdateProjectModelView(SelectedProject);
        }

        private void ActionSupprimerProjet(object parameter)
        {
            if (SelectedProject != null)
            {
                try
                {
                    Singleton.GetInstance().GetDeleteProject(SelectedProject);
                    Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AdminModelView();
                }
                catch (Exception ex)
                {
                        MessageBox.Show("Error : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Impossible de récupérer les informations du projet.");
            }
        }
        #endregion

        public void LoadAction()
        {
            AddProject = new RelayCommand(ActionAddProject);
            ModifierProjet = new RelayCommand(ActionModifierProject);
            SupprimerProjet = new RelayCommand(ActionSupprimerProjet);
        }
    }
}
