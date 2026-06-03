using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Client
    {
        private int idClient;
        private Abonnement unAbonnement;
        private String nom, prenom, mail, telephone, adresse, codePostal, ville;
        private DateTime dateNaissance;

        public Client()
        {
        }

        public Client(int idClient, int idAbonnement, string nom, string prenom, string mail, string telephone, string adresse, string codePostal, string ville, DateTime dateNaissance)
        {
            this.IdClient = idClient;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Mail = mail;
            this.Telephone = telephone;
            this.Adresse = adresse;
            this.CodePostal = codePostal;
            this.Ville = ville;
            this.DateNaissance = dateNaissance;
            this.UnAbonnement = new Abonnement().FindByID(idAbonnement);
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

        public string Nom
        {
            get
            {
                return this.nom;
            }

            set
            {
                this.nom = value;
            }
        }

        public string Prenom
        {
            get
            {
                return this.prenom;
            }

            set
            {
                this.prenom = value;
            }
        }

        public string Mail
        {
            get
            {
                return this.mail;
            }

            set
            {
                this.mail = value;
            }
        }

        public string Telephone
        {
            get
            {
                return this.telephone;
            }

            set
            {
                this.telephone = value;
            }
        }

        public string Adresse
        {
            get
            {
                return this.adresse;
            }

            set
            {
                this.adresse = value;
            }
        }

        public string CodePostal
        {
            get
            {
                return this.codePostal;
            }

            set
            {
                this.codePostal = value;
            }
        }

        public string Ville
        {
            get
            {
                return this.ville;
            }

            set
            {
                this.ville = value;
            }
        }

        public DateTime DateNaissance
        {
            get
            {
                return this.dateNaissance;
            }

            set
            {
                this.dateNaissance = value;
            }
        }

        public Abonnement UnAbonnement
        {
            get
            {
                return this.unAbonnement;
            }

            set
            {
                this.unAbonnement = value;
            }
        }

        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from CLIENT ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesClients.Add(new Client(
                        (int)dr["CLIENT_ID"],
                        (int)dr["ABONNEMENT_ID"],
                        (string)dr["NOM"],
                        (string)dr["PRENOM"],
                        (string)dr["MAIL"],
                        (string)dr["TELEPHONE"],
                        (string)dr["ADRESSE"] as string,
                        (string)dr["CODE_POSTAL"] as string,
                        (string)dr["VILLE"] as string,
                        ((DateOnly)dr["DATE_NAISSANCE"]).ToDateTime(TimeOnly.MinValue)
                    ));
                
            }
            return lesClients;
        }
    }
}
