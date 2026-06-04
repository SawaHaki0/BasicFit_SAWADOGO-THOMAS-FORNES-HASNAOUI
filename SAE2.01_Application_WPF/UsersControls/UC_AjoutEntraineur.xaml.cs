using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Npgsql; // Ne pas oublier pour la connexion PostgreSQL
using SAE2._01_Application_WPF.Classes;

namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour UC_AjoutEntraineur.xaml
    /// </summary>
    public partial class UC_AjoutEntraineur : UserControl
    {
        public UC_AjoutEntraineur()
        {
            InitializeComponent();
        }

        // 1. Gère le bouton de retour "<" vers la liste
        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                // On recharge l'UC de la liste dans le conteneur principal central
                fenetre.MainContainer.Content = new UCEntraineurs();
            }
        }

        // 2. Gère le bouton orange "Finaliser la création"
        private void btnFinaliserCreation_Click(object sender, RoutedEventArgs e)
        {
            // Récupération des données en enlevant les espaces inutiles autour
            string nom = txtSaisieNom.Text.Trim();
            string prenom = txtSaisiePrenom.Text.Trim();

            // Sécurité : On vérifie que les champs ne sont pas vides ou égaux aux placeholders d'aide
            if (string.IsNullOrEmpty(nom) || nom == "Nom" || string.IsNullOrEmpty(prenom) || prenom == "Prenom")
            {
                MessageBox.Show("Le Nom et le Prénom de l'entraîneur sont obligatoires !", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Requête SQL ajustée sur tes vrais noms de colonnes PostgreSQL (vus dans ton modèle)
            string requete = "INSERT INTO Entraineur (ENTRAINEUR_NOM, ENTRAINEUR_PRENOM) VALUES (@nom, @prenom);";

            try
            {
                // Récupération de la connexion partagée de ta SAE
                var connexion = DataAccess.GetConnection();

                NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);

                // Association des paramètres pour éviter les injections SQL
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);

                // Exécution de l'ordre SQL
                int lignesModifiees = commande.ExecuteNonQuery();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("L'entraîneur a bien été créé !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Redirection automatique vers la liste mise à jour après l'ajout
                    btnRetour_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création dans la base : " + ex.Message, "Erreur BDD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 3. Gestionnaires d'événements pour effacer le texte d'aide gris quand on clique dessus
        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "Nom" || box.Text == "Prenom")
            {
                box.Text = "";
                box.Foreground = Brushes.Black; // Le texte saisi devient noir
            }
        }

        // Remet le texte d'aide gris si l'étudiant clique ailleurs en laissant le champ vide
        private void TextBox_NoFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                box.Foreground = Brushes.Gray;
                if (box == txtSaisieNom) box.Text = "Nom";
                if (box == txtSaisiePrenom) box.Text = "Prenom";
            }
        }
    }
}