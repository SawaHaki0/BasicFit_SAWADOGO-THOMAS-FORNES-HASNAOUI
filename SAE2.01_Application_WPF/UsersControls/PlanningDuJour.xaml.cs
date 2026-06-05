using Npgsql;
using SAE2._01_Application_WPF.Classes;
using SAE2._01_Application_WPF.UsersControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

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
        private List<Seance> toutesLesSeances;

        
        private void ChargerPlanning()
        {
            int jour = (int)DateTime.Now.DayOfWeek;   
            jour = (jour == 0) ? 7 : jour;           

            try
            {
                toutesLesSeances = new Seance().FindByJour(jour);
                RowsGrid.ItemsSource = toutesLesSeances;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR :\n" + ex.Message, "Base de données",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RowsGrid.SelectedItem is Seance seance)
            {
                MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                if (fenetre != null)
                    fenetre.MainContainer.Content = new UC_Participants1stPage(seance.IdSeance, this);
            }
            else
            {
                MessageBox.Show("Sélectionnez d'abord une séance dans le planning.",
                                "Aucune séance sélectionnée",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void txtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (toutesLesSeances == null) return;

            string r = txtRecherche.Text.Trim().ToLower();

            RowsGrid.ItemsSource = toutesLesSeances.Where(s =>
                s.UnCours.NomCours.ToLower().Contains(r) ||
                s.NomSalle.ToLower().Contains(r) ||
                s.NomEntraineur.ToLower().Contains(r) ||
                s.UnCours.UneCategorie.NomCategorie.ToLower().Contains(r)
            ).ToList();
        }
    }
}