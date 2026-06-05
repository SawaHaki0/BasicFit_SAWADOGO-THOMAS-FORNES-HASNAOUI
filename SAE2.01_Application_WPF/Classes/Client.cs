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
                {
                    // 1. Gestion de l'ABONNEMENT_ID (Peut être NULL)
                    int idAbonnement;
                    if (dr["ABONNEMENT_ID"] == DBNull.Value)
                    {
                        idAbonnement = 0; // Valeur par défaut
                    }
                    else
                    {
                        idAbonnement = Convert.ToInt32(dr["ABONNEMENT_ID"]);
                    }

                    
                    string nom = dr["NOM"].ToString();
                    string prenom = dr["PRENOM"].ToString();
                    string mail = dr["MAIL"].ToString();
                    string telephone = dr["TELEPHONE"].ToString();

                    string adresse = dr["ADRESSE"] == DBNull.Value ? "" : dr["ADRESSE"].ToString();
                    string codePostal = dr["CODE_POSTAL"] == DBNull.Value ? "" : dr["CODE_POSTAL"].ToString();
                    string ville = dr["VILLE"] == DBNull.Value ? "" : dr["VILLE"].ToString();


                    DateTime dateNaissance = DateTime.MinValue;
                    if (dr["DATE_NAISSANCE"] != DBNull.Value)
                    {

                        DateOnly dateSeule = (DateOnly)dr["DATE_NAISSANCE"];


                        dateNaissance = dateSeule.ToDateTime(TimeOnly.MinValue);
                    }

                    lesClients.Add(new Client(
                        Convert.ToInt32(dr["CLIENT_ID"]),
                        idAbonnement,
                        nom,
                        prenom,
                        mail,
                        telephone,
                        adresse,
                        codePostal,
                        ville,
                        dateNaissance
                    ));
                }
                return lesClients;
            }
        }

        public List<Client> FindBySeance(int seanceId)
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(
                @"SELECT cl.*
                  FROM CLIENT cl
                  JOIN INSCRIPTION i ON i.CLIENT_ID = cl.CLIENT_ID
                  WHERE i.SEANCE_ID = @seance
                  ORDER BY cl.NOM, cl.PRENOM;"))
            {
                cmdSelect.Parameters.AddWithValue("seance", seanceId);
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {

                    int idAbonnement;
                    if (dr["ABONNEMENT_ID"] == DBNull.Value)
                    {
                        idAbonnement = 0;
                    }
                    else
                    {
                        idAbonnement = (int)dr["ABONNEMENT_ID"];
                    }

                    lesClients.Add(new Client(
                        (int)dr["CLIENT_ID"],
                        idAbonnement, 
                        (string)dr["NOM"],
                        (string)dr["PRENOM"],
                        (string)dr["MAIL"],
                        (string)dr["TELEPHONE"],
                        dr["ADRESSE"] as string,
                        dr["CODE_POSTAL"] as string,
                        dr["VILLE"] as string,
                        ((DateOnly)dr["DATE_NAISSANCE"]).ToDateTime(TimeOnly.MinValue)
                    ));
                }
            }
            return lesClients;
        }


    }
}
