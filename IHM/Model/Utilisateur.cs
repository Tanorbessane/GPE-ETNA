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
        private string token_DP;
        private string token_GG;
        private string refreshToken;
        private string login;
        private string email;
        private string mdp;
        private string role;
        private UserCredential credentiel;
        private List<Projet> lstProjet;

        public UserCredential Credentiel
        {
            get { return this.credentiel; }
            set
            {
                if (!string.Equals(this.credentiel, value))
                {
                    this.credentiel = value;
                    RaisePropertyChanged(nameof(Credentiel));
                }
            }
        }

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

        public string Token_DP
        {
            get { return this.token_DP; }
            set
            {
                if (!string.Equals(this.token_DP, value))
                {
                    this.token_DP = value;
                    RaisePropertyChanged(nameof(Token_DP));
                }
            }
        }

        public string Token_GG
        {
            get { return this.token_GG; }
            set
            {
                if (!string.Equals(this.token_GG, value))
                {
                    this.token_GG = value;
                    RaisePropertyChanged(nameof(Token_GG));
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

        public string RefreshToken
        {
            get { return this.refreshToken; }
            set
            {
                if (!string.Equals(this.refreshToken, value))
                {
                    this.refreshToken = value;
                    RaisePropertyChanged(nameof(RefreshToken));
                }
            }
        }
    }
}
