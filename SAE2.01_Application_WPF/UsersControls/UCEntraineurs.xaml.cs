using Npgsql;
using SAE2._01_Application_WPF.Classes; // Permet d'accéder à la classe Entraineur
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour UCEntraineurs.xaml
    /// </summary>
    public partial class UCEntraineurs : UserControl
    {

        public List<Entraineur> LesEntraineurs { get; set; }

        public UCEntraineurs()
        {
            InitializeComponent();

            this.LesEntraineurs = new Entraineur().FindAll();


            this.DataContext = this;
        }

        private void btnAjout_click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {

                UC_AjoutEntraineur nouvellePage = new UC_AjoutEntraineur();


                fenetrePrincipale.MainContainer.Content = nouvellePage;
            }
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

            Entraineur entraineurSelectionne = (Entraineur)dgEntraineurs.SelectedItem;


            if (entraineurSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un entraîneur dans la liste à supprimer !", "Sélection manquante", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult resultat = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer définitivement l'entraîneur {entraineurSelectionne.PrenomEntraineur} {entraineurSelectionne.NomEntraineur} ?",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );


            if (resultat == MessageBoxResult.Yes)
            {
 
                string requete = "DELETE FROM Entraineur WHERE ENTRAINEUR_ID = @id;";

                try
                {

                    var connexion = DataAccess.GetConnection();

                    NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);
                    commande.Parameters.AddWithValue("@id", entraineurSelectionne.IdEntraineur);

                    int lignesModifiees = commande.ExecuteNonQuery();

                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("L'entraîneur a bien été supprimé de la base de données.", "Suppression réussie", MessageBoxButton.OK, MessageBoxImage.Information);


                        this.LesEntraineurs = new Entraineur().FindAll();
                        dgEntraineurs.ItemsSource = this.LesEntraineurs;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression. Vérifiez si cet entraîneur n'est pas lié à un cours existant ! \nDétail : " + ex.Message, "Erreur BDD", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (dgEntraineurs.SelectedItem is Entraineur trainer)
            {
                MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                if (fenetre != null)
                    fenetre.MainContainer.Content = new UC_ModifierEntraineur(trainer, this);
            }
            else
            {
                MessageBox.Show("Sélectionnez d'abord un entraineur dans la liste.",
                                "Aucune entraineur",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}