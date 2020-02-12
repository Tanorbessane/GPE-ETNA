using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.ModelView;
using IHM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace IHM.ViewModel
{
    public class MainModelView : ObservableObject
    {
        #region Fields       
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        public MainWindow App = null;
        #endregion

        public MainModelView(MainWindow _app)
        {
            App = _app;
            PageViewModels.Add(new LoginModelView());
            CurrentPageViewModel = PageViewModels[0];

            Singleton.GetInstance().SetMainWindowViewModel(this);
        }
                   
        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                    RaisePropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }
        
        public void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }
    }
}
