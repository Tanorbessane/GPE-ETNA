using IHM.Helpers;
using IHM.ViewModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IHM.ModelView.HomePage
{
    public class HomePageModelView : ObservableObject, IPageViewModel
    {
        private DriveBase driveBaseDropbox;
        private DriveBase driveBaseGoogle;

        public HomePageModelView(DriveBase _driveBaseDropbox, DriveBase _driveBaseGoogle)
        {
            driveBaseDropbox = _driveBaseDropbox;
            driveBaseGoogle = _driveBaseGoogle;
            if (driveBaseDropbox != null)
                driveBaseDropbox.getSpace();            
        }

        public void LoadAction()
        {
            
        }
    }
}
