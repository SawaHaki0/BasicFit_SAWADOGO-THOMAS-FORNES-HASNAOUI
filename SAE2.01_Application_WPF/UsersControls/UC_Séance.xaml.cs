using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SAE2._01_Application_WPF.Classes;

namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour UC_Séance.xaml
    /// </summary>
    public partial class UC_Séance : UserControl
    {
        public UC_Séance(bool estResponsable)
        {
            InitializeComponent();

            if (!estResponsable )
            {
                btnModifier.Visibility = Visibility.Collapsed;
                btnAjouter.Visibility = Visibility.Collapsed;
                btnSupprimer.Visibility = Visibility.Collapsed;
            }
        }
        private string categorieSelectionnee = "";   // "" = toutes


        private void Filtre_Changed(object sender, SelectionChangedEventArgs e)
        {
            AppliquerFiltres();
        }

        private void FiltrerCategorie(object sender, RoutedEventArgs e)
        {
            categorieSelectionnee = (sender as Button)?.Tag as string ?? "";
            AppliquerFiltres();
            MettreEnValeurBouton(sender as Button);
        }
        private void MettreEnValeurBouton(Button actif)
        {
            if (panelCategories == null) return;

            BrushConverter bc = new BrushConverter();
            foreach (var child in panelCategories.Children)
                if (child is Button b)
                {
                    b.Background = Brushes.White;
                    b.Foreground = (Brush)bc.ConvertFrom("#555555");
                    b.BorderThickness = new Thickness(1);
                }

            if (actif != null)
            {
                actif.Background = (Brush)bc.ConvertFrom("#F26B1A");
                actif.Foreground = Brushes.White;
                actif.BorderThickness = new Thickness(0);
            }
        }

        private void AppliquerFiltres()
        {
            if (dgSeances == null) return;

            ICollectionView vue = CollectionViewSource.GetDefaultView(dgSeances.ItemsSource);
            if (vue == null) return;

            int jour = (cmbJour != null) ? cmbJour.SelectedIndex : 0;   // 0 = tous, 1..7 = Lundi..Dimanche

            vue.Filter = obj =>
            {
                if (!(obj is Seance s)) return false;

                bool okCat = string.IsNullOrEmpty(categorieSelectionnee)
                              || s?.UnCours?.UneCategorie?.NomCategorie == categorieSelectionnee;
                bool okJour = jour == 0 || s.JourSeance == jour;

                return okCat && okJour;
            };
        }

        private void btnNouvelleInscription_Click(object sender, RoutedEventArgs e)
        {
            if (dgSeances.SelectedItem is Seance seance)
            {
                MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                if (fenetre != null)
                    fenetre.MainContainer.Content = new UC_Participants1stPage(seance, this);
            }
            else
            {
                MessageBox.Show("Sélectionne d'abord une séance dans le planning.",
                                "Aucune séance sélectionnée",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnVoirStatistique_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                fenetre.MainContainer.Content = new UC_StatsPage();
            }
        }
    }
}

