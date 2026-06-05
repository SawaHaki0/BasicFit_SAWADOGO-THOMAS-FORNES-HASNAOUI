using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using SAE2._01_Application_WPF.Classes;

namespace SAE2._01_Application_WPF.UsersControls
{
    public partial class UC_StatsPage : UserControl
    {
        public UC_StatsPage()
        {
            InitializeComponent();
            ChargerStatistiques();
        }

        private void ChargerStatistiques()
        {
            try
            {
                // ----- Indicateurs globaux -----
                int totalPlaces = LireInt("SELECT COALESCE(SUM(NB_PLACES),0) FROM SEANCE;");
                int totalInscriptions = LireInt("SELECT COUNT(*) FROM INSCRIPTION;");
                int nbSeances = LireInt("SELECT COUNT(*) FROM SEANCE;");

                double tauxGlobal = totalPlaces > 0 ? (double)totalInscriptions / totalPlaces * 100 : 0;
                txtTauxGlobal.Text = Math.Round(tauxGlobal) + " %";
                txtNbSeances.Text = nbSeances.ToString();
                txtNbInscriptions.Text = totalInscriptions.ToString();

                // ----- Par jour -----
                icParJour.ItemsSource = LireStats(@"
                    SELECT se.JOUR::text AS libelle,
                           SUM(se.NB_PLACES) AS places,
                           COALESCE(SUM(nb.inscrits),0) AS inscrits
                    FROM SEANCE se
                    LEFT JOIN (SELECT SEANCE_ID, COUNT(*) AS inscrits FROM INSCRIPTION GROUP BY SEANCE_ID) nb
                           ON nb.SEANCE_ID = se.SEANCE_ID
                    GROUP BY se.JOUR
                    ORDER BY se.JOUR;", estJour: true);

                // ----- Par catégorie -----
                List<StatItem> parCat = LireStats(@"
                    SELECT cat.CATEGORIE_NOM AS libelle,
                           SUM(se.NB_PLACES) AS places,
                           COALESCE(SUM(nb.inscrits),0) AS inscrits
                    FROM SEANCE se
                    JOIN COURS c       ON c.COURS_ID = se.COURS_ID
                    JOIN CATEGORIE cat ON cat.CATEGORIE_ID = c.CATEGORIE_ID
                    LEFT JOIN (SELECT SEANCE_ID, COUNT(*) AS inscrits FROM INSCRIPTION GROUP BY SEANCE_ID) nb
                           ON nb.SEANCE_ID = se.SEANCE_ID
                    GROUP BY cat.CATEGORIE_NOM
                    ORDER BY inscrits DESC;", estJour: false);

                icParCategorie.ItemsSource = parCat;
                txtTopCategorie.Text = parCat.Count > 0 ? parCat[0].Libelle : "-";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du calcul des statistiques :\n" + ex.Message,
                                "Statistiques", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int LireInt(string sql)
        {
            DataTable dt = DataAccess.ExecuteSelect(new NpgsqlCommand(sql));
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        private List<StatItem> LireStats(string sql, bool estJour)
        {
            var liste = new List<StatItem>();
            string[] jours = { "", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };

            DataTable dt = DataAccess.ExecuteSelect(new NpgsqlCommand(sql));
            foreach (DataRow dr in dt.Rows)
            {
                int places = Convert.ToInt32(dr["places"]);
                int inscrits = Convert.ToInt32(dr["inscrits"]);
                double taux = places > 0 ? (double)inscrits / places * 100 : 0;

                string libelle = dr["libelle"].ToString();
                if (estJour && int.TryParse(libelle, out int j) && j >= 1 && j <= 7)
                    libelle = jours[j];

                liste.Add(new StatItem
                {
                    Libelle = libelle,
                    Inscrits = inscrits,
                    Places = places,
                    Taux = Math.Round(taux),
                    Detail = Math.Round(taux) + " % (" + inscrits + "/" + places + ")"
                });
            }
            return liste;
        }
    }
}