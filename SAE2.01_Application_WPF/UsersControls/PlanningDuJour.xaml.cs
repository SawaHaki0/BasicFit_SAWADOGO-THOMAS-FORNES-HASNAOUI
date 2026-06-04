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

        private void ChargerPlanning()
        {
            int jour = 1; // TEST lundi ; remettre le calcul dynamique ensuite

            try
            {
                List<Seance> lesSeances = new Seance().FindByJour(jour);
                RowsGrid.ItemsSource = lesSeances;
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
                    fenetre.MainContainer.Content = new UC_Participants1stPage(seance.IdSeance);
            }
            else
            {
                MessageBox.Show("Sélectionne d'abord un cours dans le planning.",
                                "Aucune séance sélectionnée",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}