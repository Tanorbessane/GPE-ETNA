using IHM.Helpers;
using IHM.Model;
using IHM.ModelView.Gestion_Utilisateurs;
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
    public class ListUsersModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Liste utilisateurs";
        public ICommand SearchUserBar { get; set; }
        public ICommand FicheUtilisateur { get; set; }
        public ICommand DeleteUtilisateur { get; set; }
        public ICommand AddUtilisateur { get; set; }

        public ListUsersModelView()
        {
            UsersList = Singleton.GetInstance().GetAllUtilisateur();
            LoadAction();
        }

        #region [Binding]
        private string _BtnSearch;
        public string BtnSearch
        {
            get { return this._BtnSearch; }
            set
            {
                if (!string.Equals(this._BtnSearch, value))
                {
                    this._BtnSearch = value;
                    RaisePropertyChanged(nameof(BtnSearch));
                }
            }
        }

        private string _ImgAddUtilisateur;
        public string ImgAddUtilisateur
        {
            get { return this._ImgAddUtilisateur; }
            set
            {
                if (!string.Equals(this._ImgAddUtilisateur, value))
                {
                    this._ImgAddUtilisateur = value;
                    RaisePropertyChanged(nameof(ImgAddUtilisateur));
                }
            }
        }

        private List<Utilisateur> _UsersList;
        public List<Utilisateur> UsersList
        {
            get { return this._UsersList; }
            set
            {
                if (!string.Equals(this._UsersList, value))
                {
                    this._UsersList = value;
                    RaisePropertyChanged(nameof(UsersList));
                }
            }
        }

        private string _SearchUser;
        public string SearchUser
        {
            get { return this._SearchUser; }
            set
            {
                if (!string.Equals(this._SearchUser, value))
                {
                    this._SearchUser = value;
                    RaisePropertyChanged(nameof(SearchUser));
                }
            }
        }

        private Utilisateur _UserSelected;
        public Utilisateur UserSelected
        {
            get { return this._UserSelected; }
            set
            {
                if (!string.Equals(this._UserSelected, value))
                {
                    this._UserSelected = value;
                    RaisePropertyChanged(nameof(UserSelected));
                }
            }
        }
        #endregion

        public void LoadAction()
        {
            SearchUserBar = new RelayCommand(ActionSearchUserBar);
            FicheUtilisateur = new RelayCommand(ActionFiche);
            DeleteUtilisateur = new RelayCommand(ActionDeleteUser);
            AddUtilisateur = new RelayCommand(ActionAddUtilisateur);
        }

        private void ActionAddUtilisateur(object obj)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new RegisterViewModel();
        }

        private void ActionFiche(object obj)
        {
            if (UserSelected != null)
            {
                Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new UtilisateurViewModel(UserSelected);
            }
        }
        
        private void ActionSearchUserBar(object obj)
        {
            if (SearchUser != null && SearchUser != "")
            {
                List<Utilisateur> AllUser = Singleton.GetInstance().GetAllUtilisateur();
                List<string> lstSearch = SearchUser.Split(',').ToList();
                List<Utilisateur> lstRslt = new List<Utilisateur>();

                foreach(String item in lstSearch)
                {
                    foreach(Utilisateur utilisateur in AllUser)
                    {
                        if (utilisateur.Login.ToLower().Contains(item.ToLower()))
                        {
                            lstRslt.Add(utilisateur);
                        }
                    }
                }
                UsersList = lstRslt;
            }
            else
            {
                UsersList = Singleton.GetInstance().GetAllUtilisateur();
            }
        }

        private void ActionDeleteUser(object obj)
        {
            if (UserSelected != null)
            {
                Utilisateur utilisateurSuppr = Singleton.GetInstance().GetAllUtilisateur().SingleOrDefault(x => x.Email.Equals(UserSelected.Email));
                if (utilisateurSuppr != null)
                {
                    Singleton.GetInstance().GetAllUtilisateur().Remove(utilisateurSuppr);

                    #region [Ecriture de l'utilisateur dans le fichier .JSON]
                    try
                    {
                        string test = ConfigurationSettings.AppSettings["UtilisateurJSON"];
                        using (StreamWriter file = File.CreateText(@test))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(file, Singleton.GetInstance().GetAllUtilisateur());
                            UsersList = Singleton.GetInstance().GetAllUtilisateur();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error :\" " + ex.Message);
                    }
                    #endregion
                }
            }
        }
    }
}
