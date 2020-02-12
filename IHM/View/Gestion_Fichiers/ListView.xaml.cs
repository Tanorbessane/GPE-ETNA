using IHM.Helpers;
using IHM.Model;
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
    /// Logique d'interaction pour ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        Utilisateur cUtilisateur = Singleton.GetInstance().GetUtilisateur();
        public ListView()
        {
            InitializeComponent();

            //if (cUtilisateur.Token_DP != null)
                TabDropbox.Visibility = Visibility.Visible;
            //if (cUtilisateur.Token_GG != null)
                TabGoogle.Visibility = Visibility.Visible;

            Singleton.GetInstance().GetHomeModelView()._listModelView.RefreshTab();
        }

        private void DgFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Singleton.GetInstance().GetHomeModelView()._listModelView.GetFolder();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var Date = RechercheDate.SelectedDate;
            Singleton.GetInstance().GetHomeModelView()._listModelView.Recherche_Date();
        }

        private void Periode_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var Periode = RecherchePeriode.SelectedDate;
            Singleton.GetInstance().GetHomeModelView()._listModelView.Recherche_Periode();
        }

        //private void CheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox cb = sender as CheckBox;

        //    Singleton.GetInstance().GetListModelView().LinkProject(cb.Content.ToString(), cb.IsChecked.Value);
        //}

        private void CheckBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            Singleton.GetInstance().GetListModelView().LinkProject(cb.Content.ToString(), cb.IsChecked.Value);
        }


        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Singleton.GetInstance().GetListModelView().SelectProjectByFile();
        }
    }
}
