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
        private UserControl pagePrecedente;
        private int idClient;
        public UC_Séance(bool estResponsable)
        {
            InitializeComponent();

            if (!estResponsable)
            {
                btnModifier.Visibility = Visibility.Collapsed;
                btnAjouter.Visibility = Visibility.Collapsed;

            }
        }

        public UC_Séance(bool estResponsable, int idClient, UserControl pageprecedente)
        {
            InitializeComponent();
            this.PagePrecedente = pageprecedente;
            this.idClient = idClient;

            if (!estResponsable)
            {
                btnModifier.Visibility = Visibility.Collapsed;
                btnAjouter.Visibility = Visibility.Collapsed;
            }
            
            if (this.PagePrecedente is UC_Séance)
            {
                btnModifier.Visibility = Visibility.Visible;
                btnAjouter.Visibility = Visibility.Visible;
                btnNouvelleInscription.Visibility = Visibility.Visible;
                btnVoirParticipants.Visibility = Visibility.Visible;
                btnVoirStatistique.Visibility = Visibility.Visible;
                btnRetourPage1.Visibility = Visibility.Collapsed;
                btnSupprimerIns.Visibility = Visibility.Collapsed;
            }

            if (this.PagePrecedente is UC_Participants1stPage)
            {
                btnModifier.Visibility = Visibility.Collapsed;
                btnAjouter.Visibility = Visibility.Collapsed;
                btnNouvelleInscription.Visibility = Visibility.Collapsed;
                btnVoirParticipants.Visibility = Visibility.Collapsed;
                btnVoirStatistique.Visibility = Visibility.Collapsed;
                btnRetourPage1.Visibility = Visibility.Visible;
                btnSupprimerIns.Visibility = Visibility.Visible;
            }
            dgSeances.ItemsSource = new Seance().FindByClient(idClient);
        }
        private string categorieSelectionnee = "";

        public UserControl PagePrecedente
        {
            get
            {
                return this.pagePrecedente;
            }

            set
            {
                this.pagePrecedente = value;
            }
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

            int jour = (cmbJour != null) ? cmbJour.SelectedIndex : 0;

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
                    fenetre.MainContainer.Content = new UC_Participants1stPage(seance.IdSeance, this);
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

        private void btnRetourPage1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                fenetre.MainContainer.Content = this.PagePrecedente;
            }
        }

        private void btnSupprimerIns_Click(object sender, RoutedEventArgs e)
        {
            if (dgSeances.SelectedItem is Seance seanceASupprimer)
            {

                MessageBoxResult resultat = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer l'inscription du client pour cette séance ?",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultat == MessageBoxResult.Yes)
                {
                    try
                    {
                        Inscription inscription = new Inscription();
                        bool succes = inscription.DeleteInscription(seanceASupprimer.IdSeance, this.idClient);

                        if (succes)
                        {
                            MessageBox.Show("L'inscription a bien été supprimée.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                            dgSeances.ItemsSource = new Seance().FindByClient(this.idClient);
                        }
                        else
                        {
                            MessageBox.Show("Une erreur est survenue lors de la suppression dans la base de données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un participant dans la liste à supprimer.",
                                "Aucune sélection",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {

            if (dgSeances.SelectedItem is not Seance seance)
            {
                MessageBox.Show("Sélectionnez d'abord une séance dans le planning.",
                                "Aucune séance sélectionnée",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                if (fenetre == null)
                {
                    MessageBox.Show("PROBLEME : fenetre (MainWindow) est null.");
                    return;
                }

                var page = new UC_SeanceModifier(seance, this);

                if (fenetre.MainContainer == null)
                {
                    MessageBox.Show("PROBLEME : MainContainer est null. Le conteneur s'appelle peut-être MenuContainer.");
                    return;
                }

                fenetre.MainContainer.Content = page;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR EXACTE :\n\n" + ex.GetType().Name + "\n" + ex.Message
                                + "\n\n--- INNER ---\n" + ex.InnerException?.Message
                                + "\n\n--- STACK ---\n" + ex.StackTrace,
                                "Diagnostic", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}


