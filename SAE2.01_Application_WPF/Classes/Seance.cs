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
    }
}
