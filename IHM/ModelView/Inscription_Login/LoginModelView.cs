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
    public class LoginModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Se connecter";

        #region [Command]
        public ICommand LogIn { get; set; }
        public ICommand Register { get; set; }
        #endregion

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
        #endregion
        
        #region [Constructor]
        public LoginModelView()
        {
            LoadAction();
            List<Utilisateur> items = Functions.GetFileUtilisateur();
            Singleton.GetInstance().SetListUtilisateur(items);
            Login = "";
            Mdp = "";
        }
        #endregion

        public void LoadAction()
        {
            LogIn = new RelayCommand(ActionLogiIn);
            Register = new RelayCommand(ActionResgister);
        }

        #region [Action]
        /**
         * Se connecte à l'appplication
         * */
        public void ActionLogiIn(object parameter)
        {
            System.Windows.Controls.PasswordBox p = (System.Windows.Controls.PasswordBox)parameter;
            Mdp = p.Password;

            List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
            Utilisateur u = (Utilisateur) lst.First(x => x.Login.Equals(Login) && x.MDP.Equals(Mdp));
            if (u != null)
            {
                Singleton.GetInstance().SetUtilisateur(u);
                HomeModelView HMV = new HomeModelView(u);
                Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = HMV;
            }
            else
            {
                MessageBox.Show("Aucun utilisateur trouvé.");
            }
        }

        /**
         * Renvoie à une page s'inscrire
         * */
        private void ActionResgister(object p)
        {
            Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = new RegisterViewModel();
        }
        #endregion
        
    }
}
