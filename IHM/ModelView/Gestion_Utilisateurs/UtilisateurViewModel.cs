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

namespace IHM.ModelView.Gestion_Utilisateurs
{
    public class UtilisateurViewModel : ObservableObject, IPageViewModel
    {
        public string Name => throw new NotImplementedException();
        public ICommand ModifierUtilisateur { get; set; }
        private Utilisateur utilisateur_ = new Utilisateur();

        public UtilisateurViewModel(Utilisateur _u)
        {
            _u.LstProjet = GetProjets(_u);     
            Utilisateur = _u;
            utilisateur_ = _u; //garde les traces de l'utilisateurs
            LoadAction();
        }

        private List<Projet> GetProjets(Utilisateur u)
        {
            List<Projet> lst = Singleton.GetInstance().GetAllProject();
            List<Projet> rslt = new List<Projet>();
            if (lst.Count() > 0)
            {
                foreach(Projet p in lst)
                {
                    if (p.LstUser.Where(user => user.Email.Equals(u.Email)).Count() > 0)
                    {
                        rslt.Add(p);
                    }
                }
            }
            return rslt;
        }

        private Utilisateur _utilisateur;
        public Utilisateur Utilisateur
        {
            get { return this._utilisateur; }
            set
            {
                if (!string.Equals(this._utilisateur, value))
                {
                    this._utilisateur = value;
                    RaisePropertyChanged(nameof(Utilisateur));
                }
            }
        }

        private string _ImgUser;
        public string ImgUser
        {
            get { return this._ImgUser; }
            set
            {
                if (!string.Equals(this._ImgUser, value))
                {
                    this._ImgUser = value;
                    RaisePropertyChanged(nameof(ImgUser));
                }
            }
        }

        public void LoadAction()
        {
            ModifierUtilisateur = new RelayCommand(ActionModifierUtilisateur);
        }

        private void ActionModifierUtilisateur(object obj)
        {
            List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
            Utilisateur _u = lst.FirstOrDefault(item => item.Login.Equals(utilisateur_.Login));

            _u.Login = Utilisateur.Login;
            _u.Email = Utilisateur.Email;
            _u.Role = Utilisateur.Role;

            #region [Ecriture de l'utilisateur dans le fichier .JSON]
            try
            {
                string test = ConfigurationSettings.AppSettings["UtilisateurJSON"];
                using (StreamWriter file = File.CreateText(@test))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Singleton.GetInstance().GetAllUtilisateur());
                }
                Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new ListUsersModelView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :\" " + ex.Message);
            }
            #endregion

            MessageBox.Show("L'utilisateur a été mise à jour");

        }
    }
}
