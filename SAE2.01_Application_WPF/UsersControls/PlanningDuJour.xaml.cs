using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using SAE2._01_Application_WPF.Classes;

namespace SAE2._01_Application_WPF
{
    /// <summary>
    /// Logique d'interaction pour PlanningDuJour.xaml
    /// </summary>
    public partial class PlanningDuJour : UserControl
    {
        public PlanningDuJour()
        {
            InitializeComponent();

            var culture = new CultureInfo("fr-FR");
            string date = DateTime.Now.ToString("dddd, MMMM dd, yyyy", culture);
            DateLabel.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(date);

            ChargerPlanning();
        }

        private void ChargerPlanning()
        {
            int jour = (int)DateTime.Now.DayOfWeek;
            jour = (jour == 0) ? 7 : jour;

            const string sql = @"
                SELECT TO_CHAR(se.HEURE_DEBUT,'HH24:MI') || ' - ' || TO_CHAR(se.HEURE_FIN,'HH24:MI')
         || '  |  ' || c.COURS_NOM || ' (' || cat.CATEGORIE_NOM || ')'  AS description,
       s.SALLE_NOM                                            AS salle,
       e.ENTRAINEUR_PRENOM || ' ' || e.ENTRAINEUR_NOM         AS entraineur,
       se.NB_PLACES
         - (SELECT COUNT(*) FROM INSCRIPTION i WHERE i.SEANCE_ID = se.SEANCE_ID) AS places
FROM SEANCE se
JOIN COURS      c   ON c.COURS_ID       = se.COURS_ID
JOIN CATEGORIE  cat ON cat.CATEGORIE_ID = c.CATEGORIE_ID
JOIN SALLE      s   ON s.SALLE_ID       = se.SALLE_ID
JOIN ENTRAINEUR e   ON e.ENTRAINEUR_ID  = se.ENTRAINEUR_ID
WHERE se.JOUR = @jour
ORDER BY se.HEURE_DEBUT;";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql);
                cmd.Parameters.AddWithValue("jour", jour);

                DataTable table = DataAccess.ExecuteSelect(cmd);

                var lignes = new List<SeanceDuJour>();
                foreach (DataRow row in table.Rows)
                {
                    lignes.Add(new SeanceDuJour
                    {
                        Description = row["description"].ToString(),
                        Salle = row["salle"].ToString(),
                        Entraineur = row["entraineur"].ToString(),
                        Places = Convert.ToInt32(row["places"])
                    });
                }

                RowsGrid.ItemsSource = lignes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR :\n" + ex.Message,
                                "Base de données", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}