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

namespace IHM.ModelView
{
    public class RolesModelView : ObservableObject, IPageViewModel
    {
        public RolesModelView()
        {
            LoadRoles();
            LoadAction();
            Singleton.GetInstance().SetRolesModelView(this);
        }

        private void LoadRoles()
        {
            List<Roles> items = Functions.GetFileRole();
            Singleton.GetInstance().SetListRole(items);
            LstRoles = items;
        }

        public void setLstPChecked(string nomFonctionnalite, bool isChecked)
        {
            Singleton.GetInstance().GetRoleByNom(roleSelected.Nom).lstFontionnalites.FirstOrDefault(x => x.Nom.Equals(nomFonctionnalite)).Ischecked = isChecked;

            Functions.GetFileRole();
        }

        #region [Binding]
        private List<Fonctionnalites> lstFontionnalites;
        public List<Fonctionnalites> LstFontionnalites
        {
            get { return this.lstFontionnalites; }
            set
            {
                if (!string.Equals(this.lstFontionnalites, value))
                {
                    this.lstFontionnalites = value;
                    RaisePropertyChanged(nameof(LstFontionnalites));
                }
            }
        }

        private List<Roles> lstRoles;
        public List<Roles> LstRoles
        {
            get { return this.lstRoles; }
            set
            {
                if (!string.Equals(this.lstRoles, value))
                {
                    this.lstRoles = value;
                    RaisePropertyChanged(nameof(LstRoles));
                }
            }
        }

        private Roles roleSelected;
        public Roles RoleSelected
        {
            get { return this.roleSelected; }
            set
            {
                if (!string.Equals(this.roleSelected, value))
                {
                    this.roleSelected = value;
                    RaisePropertyChanged(nameof(RoleSelected));
                    LstFontionnalites = roleSelected.lstFontionnalites;
                }
            }
        }
        #endregion

        public void LoadAction()
        {
           //
        }
    }
}
