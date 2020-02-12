using IHM.Helpers;
using IHM.View;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class AddFolderModelView : ObservableObject, IPageViewModel
    {
        private AddFolderView app;
        public ICommand SaveFolder { get; set; }

        public AddFolderModelView(AddFolderView _app)
        {
            app = _app;
            LoadAction();
        }

        #region [Binding]
        private string _Nouveau_dossier;
        public string Nouveau_dossier
        {
            get { return this._Nouveau_dossier; }
            set
            {
                if (!string.Equals(this._Nouveau_dossier, value))
                {
                    this._Nouveau_dossier = value;
                    RaisePropertyChanged(nameof(Nouveau_dossier));
                }
            }
        }

        public string Name => throw new NotImplementedException();
        #endregion

        private void ActionCreateFolder(object p)
        {
            if (Nouveau_dossier != null)
            {
               // Singleton.GetInstance().GetDBB().CreateFolder(Nouveau_dossier);
                app.Close();
            }
        }

        public void LoadAction()
        {
            SaveFolder = new RelayCommand(ActionCreateFolder);
        }
    }
}
