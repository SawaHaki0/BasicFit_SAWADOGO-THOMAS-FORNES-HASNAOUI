using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Cours
    {
        private int idCours;
        private Categorie uneCategorie;
        private String nomCours, descriptionCours;

        public Cours()
        {
        }

        public Cours(int idCours, int idCategorie, string nomCours, string descriptionCours)
        {
            this.IdCours = idCours;
            this.NomCours = nomCours;
            this.DescriptionCours = descriptionCours;
        }

        public int IdCours
        {
            get
            {
                return this.idCours;
            }

            set
            {
                this.idCours = value;
            }
        }

        public string NomCours
        {
            get
            {
                return this.nomCours;
            }

            set
            {
                this.nomCours = value;
            }
        }

        public string DescriptionCours
        {
            get
            {
                return this.descriptionCours;
            }

            set
            {
                this.descriptionCours = value;
            }
        }

        public Categorie UneCategorie
        {
            get
            {
                return this.uneCategorie;
            }

            set
            {
                this.uneCategorie = value;
            }
        }

        public List<Cours> FindAll()
        {
            List<Cours> lesCours = new List<Cours>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from COURS ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesCours.Add(new Cours(
                        (int)dr["COURS_ID"],
                        (int)dr["CATEGORIE_ID"],
                        (string)dr["COURS_NOM"],
                        (string)dr["COURS_DESCRIPTION"]
                    ));

            }
            return lesCours;
        }
    }
}
