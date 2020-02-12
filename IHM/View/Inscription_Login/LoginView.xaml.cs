using IHM.Helpers;
using IHM.Model;
using IHM.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IHM.View
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string Login = txtLogin.Text;
                string Mdp = txtPassword.Password.ToString();

                List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
                Utilisateur u = (Utilisateur)lst.First(x => x.Login.Equals(Login) && x.MDP.Equals(Mdp));
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
        }
    }
}
