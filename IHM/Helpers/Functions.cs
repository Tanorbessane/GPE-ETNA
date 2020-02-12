using IHM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IHM.Helpers
{
    class Functions
    {
        /// <summary>
        /// Creation du fichier Utilisateur JSON
        /// </summary>
        public static void CreateFileUtilisateur()
        {
            try
            {
                string test = Constant.path_utilisateur;
                using (StreamWriter file = File.CreateText(@test))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Singleton.GetInstance().GetAllUtilisateur());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :\" " + ex.Message);
            }
        }

        /// <summary>
        /// Recupération des utilisateurs du fichier JSON
        /// </summary>
        /// <returns></returns>
        public static List<Utilisateur> GetFileUtilisateur()
        {
            List<Utilisateur> items = new List<Utilisateur>();
            try
            {
                StreamReader r;
                string test = Constant.path_utilisateur;
                using (r = new StreamReader(@test))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Utilisateur>>(json);
                }
            }
            catch (Exception)
            {
                items = new List<Utilisateur>();
            }
            return items;
          }

        /// <summary>
        /// Recupération des roles du fichier JSON
        /// </summary>
        /// <returns></returns>
        public static List<Roles> GetFileRole()
        {
            List<Roles> items = new List<Roles>();
            try
            {
                StreamReader r;
                string test = Constant.path_role;
                using (r = new StreamReader(@test))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Roles>>(json);
                }
            }
            catch (Exception)
            {
                items = new List<Roles>();
            }
            return items;
        }

        /// <summary>
        /// Recupération des projet du fichier JSON
        /// </summary>
        /// <returns></returns>
        public static List<Projet> GetFileProjet()
        {
            List<Projet> items;
            try
            {
                StreamReader r;
                string test = Constant.path_projet;
                using (r = new StreamReader(@test))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Projet>>(json);
                    items.OrderByDescending(x => x.DateDeCreation).Select(x => x.Nom).ToList();
                }
            } 
            catch (Exception)
            {
                items = new List<Projet>();
            }
            return items;
        }

        /// <summary>
        /// Creation du fichier Projet JSON
        /// </summary>
        public static void CreateFileProjet()
        {
            try
            {
                string test = Constant.path_projet;
                using (StreamWriter file = File.CreateText(@test))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Singleton.GetInstance().GetAllProject());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :\" " + ex.Message);
            }
        }
    }
}
