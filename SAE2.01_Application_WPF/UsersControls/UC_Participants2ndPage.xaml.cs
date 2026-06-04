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
        // Gestionnaires d'événements pour les placeholders (textes d'aide gris)
        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "Nom" || box.Text == "Prenom" || box.Text == "Mail" ||
                box.Text == "Téléphone" || box.Text == "📅 Date de Naissance" ||
                box.Text == "Adresse" || box.Text == "Code Postal" || box.Text == "Ville")
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
                if (box == txtSaisieCodePostal) box.Text = "Code Postal";
                if (box == txtSaisieVille) box.Text = "Ville";
            }
        }

        // Gère le clic sur le bouton de retour "<"
        private void btnRetourPage1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);
            if (fenetrePrincipale != null)
            {
                fenetrePrincipale.MainContainer.Content = new UC_Participants1stPage();
            }
        }
        private void btnValiderCreation_Click(object sender, RoutedEventArgs e)
        {
            // 1. Récupération et nettoyage des textes (on vire le texte si c'est le placeholder d'aide)
            string nom = (txtSaisieNom.Text == "Nom") ? "" : txtSaisieNom.Text.Trim();
            string prenom = (txtSaisiePrenom.Text == "Prenom") ? "" : txtSaisiePrenom.Text.Trim();
            string mail = (txtSaisieMail.Text == "Mail") ? "" : txtSaisieMail.Text.Trim();
            string telephone = (txtSaisieTelephone.Text == "Téléphone") ? "" : txtSaisieTelephone.Text.Trim();
            string adresse = (txtSaisieAdresse.Text == "Adresse") ? "" : txtSaisieAdresse.Text.Trim();
            string dateTexte = (txtSaisieNaissance.Text == "📅 Date de Naissance") ? "" : txtSaisieNaissance.Text.Trim();

            // NOUVEAU : Récupération du Code Postal et de la Ville
            string cpTexte = (txtSaisieCodePostal.Text == "Code Postal") ? "" : txtSaisieCodePostal.Text.Trim();
            string ville = (txtSaisieVille.Text == "Ville") ? "" : txtSaisieVille.Text.Trim();

            // 2. Validations de sécurité
            if (nom == "" || prenom == "" || dateTexte == "")
            {
                MessageBox.Show("Le Nom, le Prénom et la Date de naissance sont obligatoires !", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Conversion sécurisée de la Date de naissance
            DateTime dateNaissance;
            if (!DateTime.TryParse(dateTexte, out dateNaissance))
            {
                MessageBox.Show("Le format de la date est incorrect (Attendu: JJ/MM/AAAA).", "Format invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Conversion sécurisée du Code Postal en entier (int) pour PostgreSQL
            int codePostal = 0;
            if (cpTexte != "")
            {
                if (!int.TryParse(cpTexte, out codePostal))
                {
                    MessageBox.Show("Le Code Postal doit être composé uniquement de chiffres !", "Format invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // 3. Requête SQL d'insertion incluant code_postal et ville
            string requete = "INSERT INTO Client (nom, prenom, mail, telephone, date_naissance, adresse, code_postal, ville) " +
                             "VALUES (@nom, @prenom, @mail, @telephone, @dateNaissance, @adresse, @codePostal, @ville);";

            try
            {
                // On récupère la connexion partagée du groupe
                var connexion = DataAccess.GetConnection();

                NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);

                // Association des paramètres
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@mail", mail);
                commande.Parameters.AddWithValue("@telephone", telephone);
                commande.Parameters.AddWithValue("@dateNaissance", dateNaissance);
                commande.Parameters.AddWithValue("@adresse", adresse);

                // Si le code postal ou la ville sont vides, on gère proprement pour la BDD
                commande.Parameters.AddWithValue("@codePostal", cpTexte == "" ? (object)DBNull.Value : codePostal);
                commande.Parameters.AddWithValue("@ville", ville == "" ? (object)DBNull.Value : ville);

                // Exécution de la requête
                int lignesModifiees = commande.ExecuteNonQuery();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Le participant a bien été ajouté !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Retour automatique à la liste (Page 1)
                    btnRetourPage1_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
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
