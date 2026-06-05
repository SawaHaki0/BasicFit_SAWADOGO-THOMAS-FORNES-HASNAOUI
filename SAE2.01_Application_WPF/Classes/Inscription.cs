using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Inscription
    {
        private int idSeance, idClient;
        private DateTime dateInscription;

        public Inscription(int idSeance, int idClient, DateTime dateInscription)
        {
            this.IdSeance = idSeance;
            this.IdClient = idClient;
            this.DateInscription = dateInscription;
        }

        public Inscription()
        {
        }

        public int IdSeance
        {
            get
            {
                return this.idSeance;
            }

            set
            {
                this.idSeance = value;
            }
        }

        public int IdClient
        {
            get
            {
                return this.idClient;
            }

            set
            {
                this.idClient = value;
            }
        }

        public DateTime DateInscription
        {
            get
            {
                return this.dateInscription;
            }

            set
            {
                this.dateInscription = value;
            }
        }

        public List<Inscription> FindAll()
        {
            List<Inscription> lesInscriptions = new List<Inscription>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from INSCRIPTION  ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesInscriptions.Add(new Inscription(
                        (int)dr["SEANCE_ID"],
                        (int)dr["CLIENT_ID"],
                        ((DateOnly)dr["DATE_INSCRIPTION"]).ToDateTime(TimeOnly.MinValue)
                    ));

            }
            return lesInscriptions;
        }

        public bool DeleteInscription(int seanceId, int clientId)
        {
            string query = "DELETE FROM INSCRIPTION WHERE SEANCE_ID = @SeanceId AND CLIENT_ID = @ClientId;";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@SeanceId", seanceId);
                cmd.Parameters.AddWithValue("@ClientId", clientId);

                try
                {
                    var connexion = DataAccess.GetConnection();
                    cmd.Connection = connexion;

                    int lignesAffectees = cmd.ExecuteNonQuery();

                    return lignesAffectees > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur lors de la suppression de l'inscription : " + ex.Message);
                }
            }
        }
    }
}
