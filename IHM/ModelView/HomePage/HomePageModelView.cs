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
        public string Name => "";

        public HomePageModelView()
        {
            Singleton.GetInstance().GetDBB().getSpace();            
        }

        public void LoadAction()
        {
            
        }
    }
}
