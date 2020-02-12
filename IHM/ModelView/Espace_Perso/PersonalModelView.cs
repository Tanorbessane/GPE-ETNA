using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class PersonalModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Espace personnel";
        public ICommand ConnecterDP { get; set; }
        public ICommand ConnecterGG{ get; set; }
        public ICommand MiseAJourUser { get; set; }

        private Utilisateur u;
        private string strAccessToken = string.Empty;
        private string strAuthenticationURL = string.Empty;

        //
        private DriveBase driveBaseGoogle;
        private DriveBase driveBaseDropbox;

        public PersonalModelView()
        {
            driveBaseGoogle = new GoogleCloud();           
            u = Singleton.GetInstance().GetUtilisateur();
            driveBaseDropbox = new DropBoxCloud();

            Login = u.Login;
            Mdp = u.MDP;
            Email = u.Email;
            Role = u.Role;
            LoadDrive();
            LoadAction();
        }

        /// <summary>
        /// Charge les drives connectés
        /// </summary>
        private void LoadDrive()
        {
            if (u.CrededentielCloudRailDropbox != null)
                strDP = "Dropbox connecté";
            if (u.CrededentielCloudRailGoogle != null)
                strGG = "Google connecté";
        }

        #region [Binding]

        private string _Login;
        public string Login
        {
            get { return this._Login; }
            set
            {
                if (!string.Equals(this._Login, value))
                {
                    this._Login = value;
                    RaisePropertyChanged(nameof(Login));
                }
            }
        }

        private string _Mdp;
        public string Mdp
        {
            get { return this._Mdp; }
            set
            {
                if (!string.Equals(this._Mdp, value))
                {
                    this._Mdp = value;
                    RaisePropertyChanged(nameof(Mdp));
                }
            }
        }

        private string _Email;
        public string Email
        {
            get { return this._Email; }
            set
            {
                if (!string.Equals(this._Email, value))
                {
                    this._Email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        private string _Role;
        public string Role
        {
            get { return this._Role; }
            set
            {
                if (!string.Equals(this._Role, value))
                {
                    this._Role = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        private string _strDP = " Connecter Droppbox";
        public string strDP
        {
            get { return this._strDP; }
            set
            {
                if (!string.Equals(this._strDP, value))
                {
                    this._strDP = value;
                    RaisePropertyChanged(nameof(strDP));
                }
            }
        }

        private string _strGG = " Connecter Google";
        public string strGG
        {
            get { return this._strGG; }
            set
            {
                if (!string.Equals(this._strGG, value))
                {
                    this._strGG = value;
                    RaisePropertyChanged(nameof(strGG));
                }
            }
        }
        
        #endregion

        public void LoadAction()
        {
            MiseAJourUser = new RelayCommand(ActionMiseAJourUser);
            ConnecterDP = new RelayCommand(ActionConnecterDropbox);
            ConnecterGG = new RelayCommand(ActionConnecterGoogle);
        }

        #region Dropbox        
        /// <summary>
        /// Ouvre une nouvelle fenêtre qui demande l'autorasition de se connecter à dropbox
        /// </summary>
        /// <param name="parameter"></param>
        private void ActionConnecterDropbox(object parameter)
        {
            string credential = driveBaseGoogle.Connect();
            u.CrededentielCloudRailDropbox = credential;
            UpdateUtilisateur();
            Singleton.GetInstance().GetListModelView().DgFiles_DP = driveBaseDropbox.GetItems();

            strGG = "Dropbox connecté";
            MessageBox.Show("Dropbox connecté");
        }

        /**
         * Met à jour l'utilisateur
         * */
        private void UpdateUtilisateur()
        {
            Functions.CreateFileUtilisateur();
        }

        #endregion

        #region Google

        /// <summary>
        /// Se connecter à google
        /// </summary>
        /// <param name="obj"></param>
        private void ActionConnecterGoogle(object obj)
        {
            try
            {
                string credential =  driveBaseGoogle.Connect();
                u.CrededentielCloudRailGoogle = credential;
                UpdateUtilisateur();
                Singleton.GetInstance().setListFilesView(new ListModelView(driveBaseDropbox, driveBaseGoogle));

                strGG = "Google connecté";
                MessageBox.Show("Google connecté");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion  

        private void ActionMiseAJourUser(object obj)
        {
            List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
            Utilisateur _u = lst.FirstOrDefault(item => item.Login.Equals(Login));
            if (_u != null && _u.Email != Email) {
                lst.FirstOrDefault(item => item.Login.Equals(Login)).Email = Email;
                Functions.CreateFileUtilisateur();
                MessageBox.Show("L'utilisateur a été mise à jour");
            }
        }
    }
}
