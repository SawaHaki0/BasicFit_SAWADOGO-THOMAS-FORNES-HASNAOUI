using Npgsql;
using SAE2._01_Application_WPF.Classes;
using System;
using System.Collections.Generic;
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

        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "Nom" || box.Text == "Prenom")
            {
                box.Text = "";
                box.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                // Remplacez 'UC_EntraineursList' par le nom exact de votre UC de la page précédente
                fenetre.MenuContainer.Content = new UCEntraineurs();
            }
        }

        private void TextBox_NoFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                box.Foreground = System.Windows.Media.Brushes.Gray;
                if (box == txtSaisieNom) box.Text = "Nom";
                if (box == txtSaisiePrenom) box.Text = "Prenom";
            }
        }

        private void btnFinaliserCreation_Click(object sender, RoutedEventArgs e)
        {
            // Récupération des données en enlevant les espaces inutiles
            string nom = txtSaisieNom.Text.Trim();
            string prenom = txtSaisiePrenom.Text.Trim();

            // Vérification si l'utilisateur a laissé les champs vides (ou s'il y a vos placeholders)
            if (string.IsNullOrEmpty(nom) || nom == "Nom" || string.IsNullOrEmpty(prenom) || prenom == "Prenom")
            {
                MessageBox.Show("Le Nom et le Prénom de l'entraîneur sont obligatoires !", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Requête SQL pour insérer l'entraîneur (adaptez le nom de la table et des colonnes à votre BDD)
            string requete = "INSERT INTO Entraineur (nom, prenom) VALUES (@nom, @prenom);";

            try
            {
                // Récupération de la connexion commune à votre groupe
                var connexion = DataAccess.GetConnection();

                // Création et configuration de la commande PostgreSQL
                NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);

                // Exécution
                int lignesModifiees = commande.ExecuteNonQuery();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("L'entraîneur a bien été créé !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Optionnel : redirection automatique vers la liste après l'ajout réussi
                    btnRetour_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création dans la base : " + ex.Message, "Erreur BDD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
