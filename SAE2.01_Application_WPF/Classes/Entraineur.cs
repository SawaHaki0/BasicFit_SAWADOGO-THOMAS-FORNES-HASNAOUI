using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Entraineur
    {
        private int idEntraineur;
        private String nomEntraineur, prenomEntraineur;

        public Entraineur()
        {
        }

        public Entraineur(int idEntraineur, string nomEntraineur, string prenomEntraineur)
        {
            this.IdEntraineur = idEntraineur;
            this.NomEntraineur = nomEntraineur;
            this.PrenomEntraineur = prenomEntraineur;
        }

        public int IdEntraineur
        {
            get
            {
                return this.idEntraineur;
            }

            set
            {
                this.idEntraineur = value;
            }
        }

        public string NomEntraineur
        {
            get
            {
                return this.nomEntraineur;
            }

            set
            {
                this.nomEntraineur = value;
            }
        }

        public string PrenomEntraineur
        {
            get
            {
                return this.prenomEntraineur;
            }

            set
            {
                this.prenomEntraineur = value;
            }
        }

        public List<Entraineur> FindAll()
        {
            List<Entraineur> lesEntraineurs = new List<Entraineur>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from ENTRAINEUR ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesEntraineurs.Add(new Entraineur(
                        (int)dr["ENTRAINEUR_ID"],
                        (string)dr["ENTRAINEUR_NOM"],
                        (string)dr["ENTRAINEUR_PRENOM"]
                    ));

            }
            return lesEntraineurs;
        }
    }
}
