using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class RegisterViewModel : ObservableObject, IPageViewModel
    {
        public string Name => "Register";
        public ICommand Inscription { get; set; }

        public RegisterViewModel()
        {
            LoadAction();
            LoadUtilisateur();            
            LstRole = LoadRole();
            Role = "Sélectionnez un rôle...";
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

        private List<string> _lstRole;
        public List<string> LstRole
        {
            get { return this._lstRole; }
            set
            {
                if (!string.Equals(this._lstRole, value))
                {
                    this._lstRole = value;
                    RaisePropertyChanged(nameof(LstRole));
                }
            }
        }
        
        #endregion

        public void ActionInscription(object parameter)
        {
            System.Windows.Controls.PasswordBox p = (System.Windows.Controls.PasswordBox)parameter;
            Mdp = p.Password;

            //Enregistrement de l'utilisateur 
            if (Login != "" && Email != "" && Role != "")
            {
                    if (Singleton.GetInstance().GetAllUtilisateur().Find(user => user.Email.Equals(Email)) != null && Singleton.GetInstance().GetAllUtilisateur().Count() != 0)
                    {
                        MessageBox.Show("Cette emai existe deja.");
                        return;
                    }

                        //ajout de l'utilisateur
                        Utilisateur Nouvelle_Utilisateur = new Utilisateur();
                        Nouvelle_Utilisateur.Login = Login;
                        Nouvelle_Utilisateur.MDP = Mdp;
                        Nouvelle_Utilisateur.Email = Email;
                        Nouvelle_Utilisateur.Token_DP = null;
                        Nouvelle_Utilisateur.Token_GG = null;
                        Nouvelle_Utilisateur.RefreshToken = null;
                        Nouvelle_Utilisateur.Role = Role;
                        Singleton.GetInstance().addUtilisateur(Nouvelle_Utilisateur);

                        Functions.CreateFileUtilisateur();

                        if (Singleton.GetInstance().GetUtilisateur() == null) // Inscription
                        {
                            Singleton.GetInstance().SetUtilisateur(Nouvelle_Utilisateur);

                            HomeModelView HMV = new HomeModelView(Nouvelle_Utilisateur);
                            HMV.IsConnect = "Se deconnecter";
                            Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = HMV;
                        }
                        else // ajout d'un utilisateur
                        {
                            MessageBox.Show("L'utilisateur a été ajouté.");
                            ListUsersModelView lstUMV = new ListUsersModelView();
                            lstUMV.UsersList = Singleton.GetInstance().GetAllUtilisateur();
                            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = lstUMV;
                        }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }

        public void LoadAction()
        {
            Inscription = new RelayCommand(ActionInscription);
        }

        private void LoadUtilisateur()
        {
            List<Utilisateur> items =  Functions.GetFileUtilisateur();
            Singleton.GetInstance().SetListUtilisateur(items);
        }

        private List<string> LoadRole()
        {
            List<string> lst = new List<string>();
            lst.Add("Sélectionnez un rôle...");
            lst.Add("Secrétaire");
            lst.Add("Developpeur");
            lst.Add("Chef de projet");
            lst.Add("Gestionnaire de cloud");
            return lst;
        }
    }
}
