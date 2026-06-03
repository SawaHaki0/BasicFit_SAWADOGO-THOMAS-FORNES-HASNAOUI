using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Application_WPF.Classes
{
    public class Abonnement
    {
        private static Dictionary<int, Abonnement> cacheAbonnements = new Dictionary<int, Abonnement>();

        private int idAbonnement;
        private String descriptionAbonnement;
        private decimal tarifAbonnement;

        public Abonnement()
        {
        }

        public Abonnement(int idAbonnement, string descriptionAbonnement, decimal tarifAbonnement)
        {
            this.IdAbonnement = idAbonnement;
            this.DescriptionAbonnement = descriptionAbonnement;
            this.TarifAbonnement = tarifAbonnement;
        }

        public int IdAbonnement
        {
            get
            {
                return this.idAbonnement;
            }

            set
            {
                this.idAbonnement = value;
            }
        }

        public string DescriptionAbonnement
        {
            get
            {
                return this.descriptionAbonnement;
            }

            set
            {
                this.descriptionAbonnement = value;
            }
        }

        public decimal TarifAbonnement
        {
            get
            {
                return this.tarifAbonnement;
            }

            set
            {
                this.tarifAbonnement = value;
            }
        }

        public List<Abonnement> FindAll()
        {
            List<Abonnement> lesAbonnements = new List<Abonnement>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from ABONNEMENT ;"))
            {
                DataTable dt = DataAccess.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesAbonnements.Add(new Abonnement(
                        (int)dr["ABONNEMENT_ID"], 
                        (String)dr["ABONNEMENT_DESCRIPTION"],
                        (decimal)dr["TARIF"]));
            }
            return lesAbonnements;
        }

        public Abonnement FindByID(int id)
        {
            if (cacheAbonnements.TryGetValue(id, out Abonnement enCache))
                return enCache;

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"select * from ABONNEMENT where ABONNEMENT_ID = @id;"))
            {
                cmdSelect.Parameters.AddWithValue("@id", id);

                using (DataTable dt = DataAccess.ExecuteSelect(cmdSelect))
                {  
                    if (dt.Rows.Count == 0)
                        return null;

                    else
                    {
                        DataRow dr = dt.Rows[0];
                        Abonnement abonnement = new Abonnement(
                            (int)dr["ABONNEMENT_ID"],
                            (string)dr["ABONNEMENT_DESCRIPTION"],
                            (decimal)dr["TARIF"]);

                        cacheAbonnements[id] = abonnement;
                        return abonnement;
                    }
                }
            }
        }
    }
}
