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
    /// Logique d'interaction pour UC_Participants2ndPage.xaml
    /// </summary>
    public partial class UC_Participants2ndPage : UserControl
    {
        public UC_Participants2ndPage()
        {
            InitializeComponent();
        }
        // Gère l'effacement du texte d'aide gris au clic
        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "Nom" || box.Text == "Prenom" || box.Text == "Mail" ||
                box.Text == "Téléphone" || box.Text == "📅 Date de Naissance" || box.Text == "Adresse")
            {
                box.Text = "";
                box.Foreground = Brushes.Black;
            }
        }

        // Gère le retour du texte d'aide gris si le champ est laissé vide
        private void TextBox_NoFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                box.Foreground = Brushes.Gray;
                if (box == txtSaisieNom) box.Text = "Nom";
                if (box == txtSaisiePrenom) box.Text = "Prenom";
                if (box == txtSaisieMail) box.Text = "Mail";
                if (box == txtSaisieTelephone) box.Text = "Téléphone";
                if (box == txtSaisieNaissance) box.Text = "📅 Date de Naissance";
                if (box == txtSaisieAdresse) box.Text = "Adresse";
            }
        }

        // Gère le clic sur le bouton de retour "<"
        private void btnRetourPage1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Retour à la page 1");
        }
        // Gère le clic sur le bouton orange de validation
        private void btnValiderCreation_Click(object sender, RoutedEventArgs e)
        {
            // 1. Récupération directe des textes des champs
            string nom = txtSaisieNom.Text.Trim();
            string prenom = txtSaisiePrenom.Text.Trim();
            string mail = txtSaisieMail.Text.Trim();
            string telephone = txtSaisieTelephone.Text.Trim();
            string adresse = txtSaisieAdresse.Text.Trim();

            // 2. Validation simple avec des 'if' classiques
            if (nom == "" || prenom == "")
            {
                MessageBox.Show("Le Nom et le Prénom sont obligatoires !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // On arrête tout ici
            }

            // 3. Préparation de la requête SQL
            string requete = "INSERT INTO Client (nom, prenom, mail, telephone, adresse) " +
                             "VALUES (@nom, @prenom, @mail, @telephone, @adresse);";

            try
            {
                // On récupère la connexion de votre classe DataAccess
                var connexion = DataAccess.GetConnection();

                // Création de la commande SQL
                NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);

                // Association des paramètres (très lisible, ligne par ligne)
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@mail", mail);
                commande.Parameters.AddWithValue("@telephone", telephone);
                commande.Parameters.AddWithValue("@adresse", adresse);

                // Exécution de la requête sur la base de données
                int lignesModifiees = commande.ExecuteNonQuery();

                // Si la ligne a bien été ajoutée
                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Le participant a bien été ajouté !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // On vide les champs du formulaire proprement
                    txtSaisieNom.Text = "";
                    txtSaisiePrenom.Text = "";
                    txtSaisieMail.Text = "";
                    txtSaisieTelephone.Text = "";
                    txtSaisieAdresse.Text = "";
                }
            }
            catch (Exception ex)
            {
                // En cas de problème informatique, on affiche juste l'erreur
                MessageBox.Show("Erreur BDD : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReinitialiserFormulaire()
        {
            txtSaisieNom.Text = "Nom"; txtSaisieNom.Foreground = Brushes.Gray;
            txtSaisiePrenom.Text = "Prenom"; txtSaisiePrenom.Foreground = Brushes.Gray;
            txtSaisieMail.Text = "Mail"; txtSaisieMail.Foreground = Brushes.Gray;
            txtSaisieTelephone.Text = "Téléphone"; txtSaisieTelephone.Foreground = Brushes.Gray;
            txtSaisieNaissance.Text = "📅 Date de Naissance"; txtSaisieNaissance.Foreground = Brushes.Gray;
            txtSaisieAdresse.Text = "Adresse"; txtSaisieAdresse.Foreground = Brushes.Gray;
        }
    }
}
