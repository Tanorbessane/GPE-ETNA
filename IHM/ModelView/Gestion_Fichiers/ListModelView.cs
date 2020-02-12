using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.View;
using IHM.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class ListModelView : ObservableObject, IPageViewModel
    {
        private Utilisateur cUtilisateur = Singleton.GetInstance().GetUtilisateur();
        //private Cloud cloud = Singleton.GetInstance().GetCloud();
      
        public ICommand Supprimer { get; set; }
        public ICommand CreateFolder { get; set; }
        public ICommand ReloadDataGrid { get; set; }
        public ICommand Upload { get; set; }
        public ICommand Recherche { get; set; }
        public ICommand RechercheDate { get; set; }
        public ICommand RecherchePeriode { get; set; }
        public ICommand Download { get; set; }
        public ICommand Open { get; set; }

        private DriveBase driveBaseDropbox;
        private DriveBase driveBaseGoogle;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ListModelView(DriveBase _driveBaseDropbox, DriveBase _driveBaseGoogle)
        {
            driveBaseDropbox = _driveBaseDropbox;
            driveBaseGoogle = _driveBaseGoogle;
            RefreshTab();
            Singleton.GetInstance().setListFilesView(this);
            LoadProject();
            LoadAction();
        }

        public void LoadAction()
        {
            Supprimer = new RelayCommand(ActionSupprimer);
           // CreateFolder = new RelayCommand(ActionCreateFolder);
            ReloadDataGrid = new RelayCommand(ActionReloadDataGrid);
            Upload = new RelayCommand(ActionUpload);
            Download = new RelayCommand(ActionDownload);
            Open = new RelayCommand(ActionOpen);
            Recherche = new RelayCommand(ActionRecherche);
            RechercheDate = new RelayCommand(ActionRechercheDate);
            RecherchePeriode = new RelayCommand(ActionRecherchePeriode);
        }

        #region [Binding dgFiles By Drive]

        private List<Fichier> dgFiles_DP;
        public List<Fichier> DgFiles_DP
        {
            get { return dgFiles_DP; }
            set
            {
                dgFiles_DP = value;
                RaisePropertyChanged(nameof(DgFiles_DP));
            }
        }

        private List<Fichier> dgFiles_GG;
        public List<Fichier> DgFiles_GG
        {
            get { return dgFiles_GG; }
            set
            {
                dgFiles_GG = value;
                RaisePropertyChanged(nameof(DgFiles_GG));
            }
        }
       
        #endregion

        #region [Binding]

        private List<Projet> lstprojet;
        public List<Projet> LstProjet
        {
            get { return lstprojet; }
            set
            {
                lstprojet = value;
                RaisePropertyChanged(nameof(LstProjet));
            }
        }

        private int tabIndex;
        public int TabIndex
        {
            get { return tabIndex; }
            set
            {
                tabIndex = value;
                RaisePropertyChanged(nameof(TabIndex));
            }
        }
        
        private string _ProjetFiltre;
        public string ProjetFiltre
        {
            get { return this._ProjetFiltre; }
            set
            {
                if (!string.Equals(this._ProjetFiltre, value))
                {
                    if (value != _ProjetFiltre)
                    {
                        RechercheMetadataByProjet(value);
                    }
                    RaisePropertyChanged(nameof(ProjetFiltre));
                }
            }
        }
               
        private List<Fichier> _Results;
        public List<Fichier> Results
        {
            get { return this._Results; }
            set
            {
                if (!string.Equals(this._Results, value))
                {
                    this._Results = value;
                    RaisePropertyChanged(nameof(Results));
                }
            }
        }

        //private List<List<Fichier>> _DgFiles;
        //public List<List<Fichier>>  DgFiles
        //{
        //    get { return this._DgFiles; }
        //    set
        //    {
        //        if (!string.Equals(this._DgFiles, value))
        //        {
        //            this._DgFiles = value;
        //            RaisePropertyChanged(nameof(DgFiles));
        //        }
        //    }
        //}

        private string _Date;
        public string Date
        {
            get { return this._Date; }
            set
            {
                if (!string.Equals(this._Date, value))
                {
                    this._Date = value;
                    RaisePropertyChanged(nameof(Date));
                }
            }
        }

        private Fichier _filesSelected;
        public Fichier filesSelected
        {
            get {          
                return this._filesSelected;
            }
            set
            {
                if (!string.Equals(this._filesSelected, value))
                {
                    this._filesSelected = value;                        
                    RaisePropertyChanged(nameof(filesSelected));
                }
            }
        }

        private string _Nom;
        public string Nom
        {
            get { return this._Nom; }
            set
            {
                if (!string.Equals(this._Nom, value))
                {
                    this._Nom = value;
                    RaisePropertyChanged(nameof(Nom));
                }
            }
        }
        
        private DateTime _dateDebut;
        public DateTime dateDebut
        {
            get { return _dateDebut; }
            set
            {
                if (!DateTime.Equals(this._dateDebut, value))
                {
                    this._dateDebut = value;
                    RaisePropertyChanged(nameof(dateDebut));
                }
            }
        }
        private DateTime _dateFin;
        public DateTime dateFin
        {
            get { return _dateFin; }
            set
            {
                if (!DateTime.Equals(this._dateFin, value))
                {
                    this._dateFin = value;
                    RaisePropertyChanged(nameof(dateFin));
                }
            }
        }

        #endregion

        #region [Methods]

        public void SelectProjectByFile()
        {
            List<Projet> lst = new List<Projet>();

            LstProjet = Singleton.GetInstance().GetAllProject();
            foreach (var item in LstProjet)
            {
                if (item.LstFiles.Exists(f => (f.IdDropbox == filesSelected.IdDropbox || f.IdGoogle == filesSelected.IdGoogle) && f.Nom.Equals(filesSelected.Nom)))
                {
                    item.IscheckedProject = true;
                }
                else
                {
                    item.IscheckedProject = false;
                }
            }
        }

        private void LoadProject()
        {
            List<Projet> items = Functions.GetFileProjet();
            Singleton.GetInstance().SetListProject(items);
            LstProjet = items;
        }

        public void LinkProject(string projet_nom, bool check_nom)
        {
             Projet p = Singleton.GetInstance().getProjetByName(projet_nom);
        
                if (p.LstFiles.Exists(f => (f.IdGoogle == filesSelected.IdGoogle && f.Nom.Equals(filesSelected)) || (f.IdDropbox == filesSelected.IdDropbox && f.Nom.Equals(filesSelected))))
                {
                        p.IscheckedProject = check_nom;
                        p.LstFiles.Add(filesSelected);
                        Singleton.GetInstance().UpdateProject(p);                   
                 }
            LoadProject();
        }

        /// <summary>
        /// Rafraichis le tableau
        /// </summary>
        public void RefreshTab()
        {
            if (driveBaseDropbox != null)
                DgFiles_DP = driveBaseDropbox.GetItems();
            if (driveBaseGoogle != null)
                DgFiles_GG = driveBaseGoogle.GetItems();
        }

        /// <summary>
        /// Récupère les items d'un dossier
        /// </summary>
        public void GetFolder()
        {
            if (filesSelected != null && filesSelected.IsFile == false)
            {
                //recupère les dossiers que de dropbox
                if (filesSelected.IdDropbox != null)
                {
                    List<Fichier> lst = driveBaseDropbox.GetItemsFolder(filesSelected.path);
                   DgFiles_DP.Clear();
                   DgFiles_DP = lst;
                }
                
                RefreshTab();
            }
        }

        /// <summary>
        /// Recherche les fichiers d'un projet
        /// </summary>
        /// <param name="value"></param>
        private void RechercheMetadataByProjet(string value)
        {
            //refresh les listes avant la recherche
            //Singleton.GetInstance().GetHomeModelView().GetFilesDropbox();
            //Singleton.GetInstance().GetHomeModelView().GetFilesGoogle();

            //recuperation du projet
            Projet projet = Singleton.GetInstance().GetAllProject().FirstOrDefault(x => x.Nom.Equals(value));

            List<Fichier> lst = new List<Fichier>();

            if (projet.LstFiles != null)
            {
                projet.LstFiles.ForEach(metadata =>
                {
                    if (metadata.IsFile == false)
                    {
                        var resultat = VerificationFichier(metadata.path);
                        if (resultat.Count() > 0)
                        {
                            resultat.ForEach(item =>
                            {
                                lst.Add(item);
                            });
                        }
                    }
                    else
                    {
                        lst.Add(metadata);
                    }
                });
            }
            
            DgFiles_DP = lst.FindAll(fichier => fichier.IdDropbox != null).ToList(); //DP
            DgFiles_GG = lst.FindAll(fichier => fichier.IdGoogle != null).ToList(); //GG

        }

        /// <summary>
        /// Vérifie si un dossier possède des autres dossiers ainsi que ses fichiers
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NomFichier"></param>
        /// <returns></returns>
        private List<Fichier> VerificationFichier(string path)
        {
            List<Fichier> rslt = new List<Fichier>();
            List<Fichier> lst = driveBaseDropbox.GetItemsFolder(path);
            if (lst.Count() > 0)
            {
                lst.ForEach(doc =>
                {
                    if (doc.IsFile == false)
                    {
                        VerificationFichier(doc.path);
                    }
                    else
                    {
                        rslt.Add(doc);
                    }
                });
            }
            return rslt;
        }

        #endregion

        #region [Action ICommand]

        /// <summary>
        /// Supprimer un dossier ou un fichier
        /// </summary>
        /// <param name="parameter"></param>
        private void ActionSupprimer(object parameter)
        {
            if (filesSelected != null)
            {
                MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer  " + filesSelected.Nom + "?", "Infos", MessageBoxButton.YesNo);
                bool isDelete = false;

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (TabIndex == 1)
                        {
                            isDelete = driveBaseDropbox.Delete(filesSelected.path);
                        }
                        else { 
                            isDelete = driveBaseGoogle.Delete(filesSelected.path);
                        }

                        if (isDelete)
                        {
                            RefreshTab();
                            MessageBox.Show("Fichier supprimé.");
                        }
                        else
                        {
                            MessageBox.Show("Erreur le fichier n'a pas pu etre supprimé.");
                        }

                        break;
                    case MessageBoxResult.No:
                        //
                        break;
                }
            }
            else
            {
                MessageBox.Show("Aucun fichier(s) sélectioné(s).");
            }
        }

        /// <summary>
        /// Lie un fichier à un projet 
        /// Partage le fichier/dossier aux utilisateurs
        /// </summary>
        /// <param name="parameter"></param>
        //private void ActionCreateFolder(object parameter)
        //{
        //    try
        //    {
        //        Drive DriveChecked = Drive.GG;

        //        if (Drive.DP == DriveChecked)
        //        {
        //            Singleton.GetInstance().GetCloud().FolderExists(Drive.DP, "/", "test__");
        //        }
        //        else
        //        {
        //            Singleton.GetInstance().GetCloud().CreateFolder(Drive.GG, "/", "test__");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error:\"" + ex.Message);
        //    }
        //}

        /// <summary>
        /// Reload Grid
        /// </summary>
        /// <param name="parameter"></param>
        private void ActionReloadDataGrid(object parameter)
        {
            if (cUtilisateur.CrededentielCloudRailDropbox != null)
            {
                DgFiles_DP = driveBaseDropbox.GetItems();
            }
            if (cUtilisateur.CrededentielCloudRailGoogle != null)
            {
                DgFiles_GG = driveBaseGoogle.GetItems();
            }

            RefreshTab();
        }

        /// <summary>
        /// Upload un fichier
        /// </summary>
        /// <param name="paramater"></param>
        private void ActionUpload(object paramater)
        {
            bool result = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string SourceFilePath = "/";
            if (openFileDialog.ShowDialog() == true)
            {
                SourceFilePath = openFileDialog.FileName;
                if (TabIndex == 1)
                {
                    result = driveBaseDropbox.Upload(SourceFilePath);
                }
                else
                {
                    result = driveBaseGoogle.Upload(SourceFilePath);
                }

                if (result)
                    RefreshTab();
                MessageBox.Show("Fichier importé.");
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="paramater"></param>
        private void ActionDownload(object paramater)
        {

            if (filesSelected != null)
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = filesSelected.Nom;                
                if (saveFileDialog.ShowDialog() == true)
                {
                    var DownloadFolderPath = saveFileDialog.FileName.Replace("\\", "/");
                    if (tabIndex == 0)
                    {
                        driveBaseGoogle.Download(filesSelected.path, DownloadFolderPath);
                    }
                    else
                    {
                        driveBaseGoogle.Download(filesSelected.path, DownloadFolderPath);
                    }
                }               
            }
            else
            {
                MessageBox.Show("Aucun fichier sélectionné");
            }
    }

        /// <summary>
        /// Action qui visualise un fichier
        /// </summary>
        /// <param name="paramater"></param>
        private void ActionOpen(object paramater)
        {
            if (filesSelected != null)
            {
                if (TabIndex == 1)
                {
                    OpenFileDropbox();
                }
                else
                {
                    bool result = driveBaseGoogle.Watch(filesSelected.Nom, filesSelected.IdGoogle);
                    if (!result)
                        MessageBox.Show("Impossible d'ouvrir ce fichier.");
                }
            }
            else
            {
                MessageBox.Show("Aucun fichier(s) sélectioné(s).");
            }
        }
        
        /// <summary>
        /// Permet de visualiser un fichier dans dropbox
        /// </summary>
        private void OpenFileDropbox()
        {
            if (filesSelected.PreviewUrl == null)
            {
                driveBaseDropbox.Download(filesSelected.path, System.IO.Path.GetTempPath() + filesSelected.Nom);
                System.Diagnostics.Process.Start(System.IO.Path.GetTempPath() + filesSelected.Nom);
            }
            else
            {
                System.Diagnostics.Process.Start(filesSelected.PreviewUrl);
            }
        }

        #endregion

        #region SALAH
        // search Files
        private void ActionRecherche(object par)
        {
            string nomRechercher = Nom;
            Results = new List<Fichier>();
            char[] delimiters = new char[] { ' ', ',', '.', ':', '\t' };
            string[] words = Nom.Split(delimiters);
            bool trouve = false;

            if (DgFiles_DP.Count() > 0)
            {
                foreach (Fichier item in DgFiles_DP)
                {

                    if (item.Nom.Contains(nomRechercher))
                    {
                        trouve = true;
                        Results.Add(item);
                    }
                    DgFiles_DP = Results;
                }
                if (trouve == false)
                {
                    MessageBox.Show("Le fichier avec le nom indiqué n’existe pas");
                }
                else
                {
                    RefreshTab();
                }

            }
        }


        public void Recherche_Periode()
        {
            Results = new List<Fichier>();
            var lstFilesDropbox = driveBaseDropbox.GetItems();

            if (dateDebut < dateFin)
            {
                throw new ArgumentException("endDate doit être supérieur ou égal à  startDate");
            }

            foreach (Fichier item in lstFilesDropbox)
            {
                if ((item.DateDeCreation != null && item.DateDeCreation > dateFin && item.DateDeCreation > dateDebut) ||
                                        (item.DateInvitation != null && item.DateInvitation > this.dateFin && item.DateInvitation > dateDebut))
                {
                    dateDebut = dateDebut.AddDays(1);
                    Console.WriteLine(Results);
                    Results.Add(item);
                }
            }
           DgFiles_DP = Results;
            RefreshTab();

        }

        private void ActionRecherchePeriode(object obj)
        {
            Recherche_Periode();
        }
        public void Recherche_Date()
        {
            string dateDebut = this.Date;
            char[] delimiters = new char[] { '/', ' ' };
            string[] words = dateDebut.Split(delimiters);
            int month = int.Parse(words[0]);
            int day = int.Parse(words[1]);
            int year = int.Parse(words[2]);

            Results = new List<Fichier>();
            bool trouve = false;

            var lstFilesDropbox = driveBaseDropbox.GetItems();

            foreach (Fichier item in lstFilesDropbox)
            {
                if (item.DateDeCreation != null)
                {
                    if (item.DateDeCreation.Value.Year == year && item.DateDeCreation.Value.Month == month && item.DateDeCreation.Value.Day == day)
                    {
                        trouve = true;
                        Results.Add(item);
                        Console.WriteLine(Results);
                    }
                }

                if (item.DateInvitation != null)
                {
                    if (item.DateInvitation.Value.Year == year && item.DateInvitation.Value.Month == month && item.DateInvitation.Value.Day == day)
                    {
                        trouve = true;
                        Results.Add(item);
                        Console.WriteLine(Results);

                    }
                }
            }
            if (trouve == false)
            {
                MessageBox.Show("La date séléctioné  n’existe pas");
            }

            DgFiles_DP = Results;
            RefreshTab();
        }

        private void ActionRechercheDate(object obj)
        {
            Recherche_Date();
        }
        #endregion
    }
}