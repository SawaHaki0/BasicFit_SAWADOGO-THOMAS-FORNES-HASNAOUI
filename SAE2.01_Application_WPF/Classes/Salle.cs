using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Salle
    {
        private int idSalle, nbPlaces;
        private String nomSalle;

        public Salle()
        {
        }

        public Salle(int idSalle, int nbPlaces, string nomSalle)
        {
            this.IdSalle = idSalle;
            this.NbPlaces = nbPlaces;
            this.NomSalle = nomSalle;
        }

        public int IdSalle
        {
            get
            {
                return this.idSalle;
            }

            set
            {
                this.idSalle = value;
            }
        }

        public int NbPlaces
        {
            get
            {
                return this.nbPlaces;
            }

            set
            {
                this.nbPlaces = value;
            }
        }

        public string NomSalle
        {
            get
            {
                return this.nomSalle;
            }

            set
            {
                this.nomSalle = value;
            }
        }

        public List<Salle> FindAll()
        {
            List<Salle> lesSalles = new List<Salle>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from SALLE ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesSalles.Add(new Salle(
                        (int)dr["SALLE_ID"],
                        (int)dr["SALLE_NOM"],
                        (string)dr["SALLE_NB_PLACES"]
                    ));

            }
            return lesSalles;
        }
    }
}
