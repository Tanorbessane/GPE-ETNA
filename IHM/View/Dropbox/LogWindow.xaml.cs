using Dropbox.Api;
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
using System.Windows.Shapes;

namespace GPE
{
    /// <summary>
    /// Logique d'interaction pour LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        #region Variables  
        private const string RedirectUri = "https://localhost/authorize";
        private string DBAppKey = string.Empty;
        private string DBAuthenticationURL = string.Empty;
        private string DBoauth2State = string.Empty;
        #endregion

        #region Properties  
        public string AccessToken { get; private set; }

        public string UserId { get; private set; }

        public bool Result { get; private set; }
        #endregion


        public LogWindow(string AppKey, string AuthenticationURL, string oauth2State)
        {
            InitializeComponent();
            DBAppKey = AppKey;
            DBAuthenticationURL = AuthenticationURL;
            DBoauth2State = oauth2State;
        }

        public void Navigate()
        {
            try
            {
                if (!string.IsNullOrEmpty(DBAppKey))
                {
                    Uri authorizeUri = new Uri(DBAuthenticationURL);
                    Browser.Navigate(authorizeUri);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(Navigate));
            // Navigate();  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void Browser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (!e.Uri.AbsoluteUri.ToString().StartsWith(RedirectUri.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // we need to ignore all navigation that isn't to the redirect uri.  
                return;
            }


            try
            {

                OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(e.Uri);
                if (result.State != DBoauth2State)
                {
                    return;
                }

                this.AccessToken = result.AccessToken;
                this.Uid = result.Uid;
                this.Result = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                e.Cancel = true;
                this.Close();
            }
        }
    }
}
