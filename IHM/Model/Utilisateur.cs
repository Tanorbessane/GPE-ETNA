using Google.Apis.Auth.OAuth2;
using IHM.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Utilisateur : Base
    {
        private string image ;
        private string login;
        private string email;
        private string mdp;
        private string role;
        private List<Projet> lstProjet;

        //cloudRail
        private string crededentielCloudRailDropbox;
        public string CrededentielCloudRailDropbox
        {
            get { return this.crededentielCloudRailDropbox; }
            set
            {
                if (!string.Equals(this.crededentielCloudRailDropbox, value))
                {
                    this.crededentielCloudRailDropbox = value;
                    RaisePropertyChanged(nameof(CrededentielCloudRailDropbox));
                }
            }
        }

        private string crededentielCloudRailGoogle;
        public string CrededentielCloudRailGoogle
        {
            get { return this.crededentielCloudRailGoogle; }
            set
            {
                if (!string.Equals(this.crededentielCloudRailGoogle, value))
                {
                    this.crededentielCloudRailGoogle = value;
                    RaisePropertyChanged(nameof(CrededentielCloudRailGoogle));
                }
            }
        }
        //

        public string Image
        {
            get { return this.image; }
            set
            {
                if (!string.Equals(this.image, value))
                {
                    this.image = value;
                    RaisePropertyChanged(nameof(Image));
                }
            }
        }

        public List<Projet> LstProjet
        {
            get { return this.lstProjet; }
            set
            {
                if (!string.Equals(this.lstProjet, value))
                {
                    this.lstProjet = value;
                    RaisePropertyChanged(nameof(LstProjet));
                }
            }
        }

        public string Login
        {
            get { return this.login; }
            set
            {
                if (!string.Equals(this.login, value))
                {
                    this.login = value;
                    RaisePropertyChanged(nameof(Login));
                }
            }
        }

        public string Email
        {
            get { return this.email; }
            set
            {
                if (!string.Equals(this.email, value))
                {
                    this.email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        public string MDP
        {
            get { return this.mdp; }
            set
            {
                if (!string.Equals(this.mdp, value))
                {
                    this.mdp = value;
                    RaisePropertyChanged(nameof(MDP));
                }
            }
        }

        public string Role
        {
            get { return this.role; }
            set
            {
                if (!string.Equals(this.role, value))
                {
                    this.role = value;
                    RaisePropertyChanged(nameof(Role));
                }
            }
        }

    }
}
