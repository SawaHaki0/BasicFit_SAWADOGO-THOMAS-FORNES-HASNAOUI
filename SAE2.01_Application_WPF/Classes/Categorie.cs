using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Categorie
    {
        private static Dictionary<int, Categorie> cacheCategories = new Dictionary<int, Categorie>();

        private int idCategorie;
        private String nomCategorie, descriptionCategorie;

        public Categorie()
        {
        }

        public Categorie(string nomCategorie, string descriptionCategorie)
        {
            this.NomCategorie = nomCategorie;
            this.DescriptionCategorie = descriptionCategorie;
        }


        public Categorie(int idCategorie, string nomCategorie, string descriptionCategorie)
        {
            this.IdCategorie = idCategorie;
            this.NomCategorie = nomCategorie;
            this.DescriptionCategorie = descriptionCategorie;
        }

        public int IdCategorie
        {
            get
            {
                return this.idCategorie;
            }

            set
            {
                this.idCategorie = value;
            }
        }

        public string NomCategorie
        {
            get
            {
                return this.nomCategorie;
            }

            set
            {
                this.nomCategorie = value;
            }
        }

        public string DescriptionCategorie
        {
            get
            {
                return this.descriptionCategorie;
            }

            set
            {
                this.descriptionCategorie = value;
            }
        }

        public List<Categorie> FindAll()
        {
            List<Categorie> lesCategories = new List<Categorie>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from CATEGORIE ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesCategories.Add(new Categorie(
                        (int)dr["CATEGORIE_ID"],
                        (String)dr["CATEGORIE_NOM"],
                        (String)dr["CATEGORIE_DESCRIPTION"]));
            }
            return lesCategories;
        }

        public Categorie FindByID(int id)
        {
            if (cacheCategories.TryGetValue(id, out Categorie enCache))
                return enCache;

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"select * from CATEGORIE where CATEGORIE_ID = @id;"))
            {
                cmdSelect.Parameters.AddWithValue("@id", id);

                using (DataTable dt = DataAccess.ExecuteSelect(cmdSelect))
                {
                    if (dt.Rows.Count == 0)
                        return null;

                    else
                    {
                        DataRow dr = dt.Rows[0];
                        Categorie categorie = new Categorie(
                            (int)dr["CATEGORIE_ID"],
                            (string)dr["CATEGORIE_NOM"],
                            (string)dr["CATEGORIE_DESCRIPTION"]);

                        cacheCategories[id] = categorie;
                        return categorie;
                    }
                }
            }
        }
    }
}
