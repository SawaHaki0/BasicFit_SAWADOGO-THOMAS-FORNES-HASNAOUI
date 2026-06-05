using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Seance
    {
        private int idSeance;
        private Cours unCours;
        private Entraineur unEntraineur;
        private Salle uneSalle;
        private int jourSeance;
        private TimeOnly heureDebut, heureFin;
        private int nbPlaces;
        private List<Client> participantsSeance = new List<Client>();

        public Seance()
        {
        }

        public Seance(int idSeance, int idCours, int idEntraineur, int idSalle, int jourSeance, TimeOnly heureDebut, TimeOnly heureFin, int nbPlaces)
        {
            this.IdSeance = idSeance;
            this.JourSeance = jourSeance;
            this.HeureDebut = heureDebut;
            this.HeureFin = heureFin;
            this.NbPlaces = nbPlaces;
            this.UnCours = new Cours().FindByID(idCours);
            this.UnEntraineur = new Entraineur().FindByID(idEntraineur);
            this.UneSalle = new Salle().FindByID(idSalle);
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

        public int JourSeance
        {
            get
            {
                return this.jourSeance;
            }

            set
            {
                this.jourSeance = value;
            }
        }

        public TimeOnly HeureDebut
        {
            get
            {
                return this.heureDebut;
            }

            set
            {
                this.heureDebut = value;
            }
        }

        public TimeOnly HeureFin
        {
            get
            {
                return this.heureFin;
            }

            set
            {
                this.heureFin = value;
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

        public Cours UnCours
        {
            get
            {
                return this.unCours;
            }

            set
            {
                this.unCours = value;
            }
        }

        public Entraineur UnEntraineur
        {
            get
            {
                return this.unEntraineur;
            }

            set
            {
                this.unEntraineur = value;
            }
        }

        public Salle UneSalle
        {
            get
            {
                return this.uneSalle;
            }

            set
            {
                this.uneSalle = value;
            }
        }

        public List<Client> ParticipantsSeance
        {
            get
            {
                return this.participantsSeance;
            }

            set
            {
                this.participantsSeance = value;
            }
        }
        public string JourLibelle
        {
            get
            {
                string[] jours = { "", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
                return (JourSeance >= 1 && JourSeance <= 7) ? jours[JourSeance] : "";
            }
        }

        public string Horaire =>
            HeureDebut.ToString("HH:mm") + " - " + HeureFin.ToString("HH:mm");

        public string Description =>
            Horaire + "  |  " + UnCours.NomCours + " (" + UnCours.UneCategorie.NomCategorie + ")";

        public string NomSalle => UneSalle.NomSalle;

        public string NomEntraineur => UnEntraineur.PrenomEntraineur + " " + UnEntraineur.NomEntraineur;

        public int PlacesRestantes => NbPlaces - ParticipantsSeance.Count;


        public List<Seance> FindAll()
        {
            List<Seance> lesSeances = new List<Seance>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from SEANCE ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesSeances.Add(new Seance(
                        (int)dr["SEANCE_ID"],
                        (int)dr["COURS_ID"],
                        (int)dr["ENTRAINEUR_ID"],
                        (int)dr["SALLE_ID"],
                        (int)dr["JOUR"],
                        (TimeOnly)dr["HEURE_DEBUT"],
                        (TimeOnly)dr["HEURE_FIN"],
                        (int)dr["NB_PLACES"]
                    ));

            }
            return lesSeances;
        }
        public List<Seance> FindByJour(int jour)
        {
            List<Seance> lesSeances = new List<Seance>();
            using (NpgsqlCommand cmdSelect =
                new NpgsqlCommand("SELECT * FROM SEANCE WHERE JOUR = @jour ORDER BY HEURE_DEBUT;"))
            {
                cmdSelect.Parameters.AddWithValue("jour", jour);
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    Seance s = new Seance(
                        (int)dr["SEANCE_ID"],
                        (int)dr["COURS_ID"],
                        (int)dr["ENTRAINEUR_ID"],
                        (int)dr["SALLE_ID"],
                        (int)dr["JOUR"],
                        (TimeOnly)dr["HEURE_DEBUT"],
                        (TimeOnly)dr["HEURE_FIN"],
                        (int)dr["NB_PLACES"]
                    );
                    s.ParticipantsSeance = new Client().FindBySeance(s.IdSeance);
                    lesSeances.Add(s);
                }
            }
            return lesSeances;
        }

        public List<Seance> FindByClient(int client)
        {
            List<Seance> lesSeances = new List<Seance>();

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(
                "SELECT s.* FROM SEANCE s JOIN INSCRIPTION i ON s.SEANCE_ID = i.SEANCE_ID WHERE i.CLIENT_ID = @ClientId;"))
            {
                cmdSelect.Parameters.AddWithValue("@ClientId", client);

                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    Seance s = new Seance(
                        (int)dr["SEANCE_ID"],
                        (int)dr["COURS_ID"],
                        (int)dr["ENTRAINEUR_ID"],
                        (int)dr["SALLE_ID"],
                        (int)dr["JOUR"],
                        (TimeOnly)dr["HEURE_DEBUT"],
                        (TimeOnly)dr["HEURE_FIN"],
                        (int)dr["NB_PLACES"]
                    );
                    lesSeances.Add(s);
                }
            }
            return lesSeances;
        }

        public int Update()
        {
            int nb = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE SEANCE SET COURS_ID = @cours, ENTRAINEUR_ID = @entr, SALLE_ID = @salle, " +
                "JOUR = @jour, HEURE_DEBUT = @hd, HEURE_FIN = @hf, NB_PLACES = @nb " +
                "WHERE SEANCE_ID = @id;"))
            {
                cmd.Parameters.AddWithValue("@cours", this.UnCours.IdCours);
                cmd.Parameters.AddWithValue("@entr", this.UnEntraineur.IdEntraineur);
                cmd.Parameters.AddWithValue("@salle", this.UneSalle.IdSalle);
                cmd.Parameters.AddWithValue("@jour", this.JourSeance);
                cmd.Parameters.AddWithValue("@hd", this.HeureDebut);
                cmd.Parameters.AddWithValue("@hf", this.HeureFin);
                cmd.Parameters.AddWithValue("@nb", this.NbPlaces);
                cmd.Parameters.AddWithValue("@id", this.IdSeance);
                nb = DataAccess.ExecuteUpdate(cmd);
            }
            return nb;
        }
    }
}
