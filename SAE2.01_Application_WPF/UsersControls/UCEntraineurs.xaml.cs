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
        // Cette propriété publique va alimenter le ItemsSource du DataGrid dans le XAML
        public List<Entraineur> LesEntraineurs { get; set; }

        public UCEntraineurs()
        {
            InitializeComponent();

            // 1. On charge la liste de tous les entraîneurs depuis la base de données
            this.LesEntraineurs = new Entraineur().FindAll();

            // 2. On lie ce code C# au XAML grâce au DataContext
            this.DataContext = this;
        }

        // Gère le clic sur le bouton "Ajouter un entraîneur"
        private void btnAjout_click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {
                // On instancie l'UserControl d'ajout
                UC_AjoutEntraineur nouvellePage = new UC_AjoutEntraineur();

                // On l'affiche proprement au centre de l'application dans le MainContainer
                fenetrePrincipale.MainContainer.Content = nouvellePage;
            }
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            // 1. On récupère l'entraîneur sélectionné dans le DataGrid
            Entraineur entraineurSelectionne = (Entraineur)dgEntraineurs.SelectedItem;

            // 2. Sécurité : On vérifie qu'une ligne est bien sélectionnée
            if (entraineurSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un entraîneur dans la liste à supprimer !", "Sélection manquante", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Fenêtre de confirmation (Oui / Non) pour valider le choix
            MessageBoxResult resultat = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer définitivement l'entraîneur {entraineurSelectionne.PrenomEntraineur} {entraineurSelectionne.NomEntraineur} ?",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            // Si l'utilisateur clique sur "Oui"
            if (resultat == MessageBoxResult.Yes)
            {
                // Requête SQL utilisant l'ID de l'entraîneur (Nom de colonne vu dans ton modèle)
                string requete = "DELETE FROM Entraineur WHERE ENTRAINEUR_ID = @id;";

                try
                {
                    // Récupération de la connexion partagée du groupe
                    var connexion = DataAccess.GetConnection();

                    NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);
                    commande.Parameters.AddWithValue("@id", entraineurSelectionne.IdEntraineur);

                    // Exécution du DELETE
                    int lignesModifiees = commande.ExecuteNonQuery();

                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("L'entraîneur a bien été supprimé de la base de données.", "Suppression réussie", MessageBoxButton.OK, MessageBoxImage.Information);

                        // 4. RAFRAÎCHISSEMENT : On recharge la liste et on met à jour le tableau
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