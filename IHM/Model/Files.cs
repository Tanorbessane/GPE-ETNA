using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Fichier
    {

        public int Id{ get; set; }
        public string IdDropbox{ get; set; }
        public string IdGoogle { get; set; }
        public string IMG { get; set; }
        public string Nom{ get; set; }
        public string Type{ get; set; }
        public DateTime? DateDeCreation{ get; internal set; }
        public DateTime? ModifieLe { get; internal set; }
        public string Taille{ get; set; }
        public long? Size { get; set; }
        public long? Version { get; set; }
        public bool IsFile { get; set; }
        public string PreviewUrl { get; internal set; }
        public DateTime? DateInvitation { get; internal set; }
        public string MimeType { get; set; }
        public string path;
        
        public Fichier()
        {

        }

        public Fichier(int _Id, string _IdDropbox, string _IMG, string _Nom, string _Type, DateTime? _DateDeCreation, DateTime? _ModifieLe, string _Taille, bool _IsFile)
        {
            Id = _Id;
            IdDropbox = _IdDropbox;
            IMG = _IMG;
            Nom = _Nom;
            Type = _Type;
            DateDeCreation = _DateDeCreation;
            ModifieLe = _ModifieLe;
            Taille = _Taille;
            IsFile = _IsFile;
        }

        public Fichier(string _IdDropbox, string _Nom, string _IMG, string _Type, DateTime? _DateDeCreation, DateTime? _ModifieLe, string _Taille, bool _IsFile)
        {
            IdDropbox = _IdDropbox;
            Nom = _Nom;
            IMG = _IMG;
            Type = _Type;
            DateDeCreation = _DateDeCreation;
            ModifieLe = _ModifieLe;
            Taille = _Taille;
            IsFile = _IsFile;
        }
    }
}
