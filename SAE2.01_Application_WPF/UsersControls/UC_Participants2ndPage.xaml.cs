using Npgsql;
using SAE2._01_Application_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private UserControl pagePrecedente;

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

        public UC_Participants2ndPage(UserControl pagePrecedente)
        {
            InitializeComponent();
            this.PagePrecedente = pagePrecedente;
        }
        
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

        
        private void btnRetourPage1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);
            if (fenetrePrincipale != null)
            {
                fenetrePrincipale.MainContainer.Content = this.PagePrecedente;
            }
        }
        private void btnValiderCreation_Click(object sender, RoutedEventArgs e)
        {
            
            string nom = txtSaisieNom.Text.Trim();
            if (nom == "Nom") { nom = ""; }

            string prenom = txtSaisiePrenom.Text.Trim();
            if (prenom == "Prenom") { prenom = ""; }

            string mail = txtSaisieMail.Text.Trim();
            if (mail == "Mail") { mail = ""; }

            string telephone = txtSaisieTelephone.Text.Trim();
            if (telephone == "Téléphone") { telephone = ""; }

            string adresse = txtSaisieAdresse.Text.Trim();
            if (adresse == "Adresse") { adresse = ""; }

            string dateTexte = txtSaisieNaissance.Text.Trim();
            if (dateTexte == "📅 Date de Naissance") { dateTexte = ""; }

            string cpTexte = txtSaisieCodePostal.Text.Trim();
            if (cpTexte == "Code Postal") { cpTexte = ""; }

            string ville = txtSaisieVille.Text.Trim();
            if (ville == "Ville") { ville = ""; }


            if (nom == "" || prenom == "" || dateTexte == "")
            {
                MessageBox.Show("Le Nom, le Prénom et la Date de naissance sont obligatoires !", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime dateNaissance;
            if (DateTime.TryParse(dateTexte, out dateNaissance) == false)
            {
                MessageBox.Show("Le format de la date est incorrect (Attendu: JJ/MM/AAAA).", "Format invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cpTexte != "")
            {
                int verificationNumerique;
                if (int.TryParse(cpTexte, out verificationNumerique) == false)
                {
                    MessageBox.Show("Le Code Postal doit être composé uniquement de chiffres !", "Format invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }



            string requete = "INSERT INTO Client (nom, prenom, mail, telephone, date_naissance, adresse, code_postal, ville) " +
                             "VALUES (@nom, @prenom, @mail, @telephone, @dateNaissance, @adresse, @codePostal, @ville);";

            try
            {

                var connexion = DataAccess.GetConnection();

                using (NpgsqlCommand commande = new NpgsqlCommand(requete, connexion))
                {

                    commande.Parameters.AddWithValue("@nom", nom);
                    commande.Parameters.AddWithValue("@prenom", prenom);
                    commande.Parameters.AddWithValue("@dateNaissance", DateOnly.FromDateTime(dateNaissance));


                    if (mail == "") { commande.Parameters.AddWithValue("@mail", DBNull.Value); } else { commande.Parameters.AddWithValue("@mail", mail); }
                    if (telephone == "") { commande.Parameters.AddWithValue("@telephone", DBNull.Value); } else { commande.Parameters.AddWithValue("@telephone", telephone); }
                    if (adresse == "") { commande.Parameters.AddWithValue("@adresse", DBNull.Value); } else { commande.Parameters.AddWithValue("@adresse", adresse); }
                    if (ville == "") { commande.Parameters.AddWithValue("@ville", DBNull.Value); } else { commande.Parameters.AddWithValue("@ville", ville); }


                    object valeurCodePostal = DBNull.Value;
                    if (cpTexte != "")
                    {
                        int cpConverti;
                        if (int.TryParse(cpTexte, out cpConverti) == true)
                        {
                            valeurCodePostal = cpConverti;
                        }
                    }
                    commande.Parameters.AddWithValue("@codePostal", valeurCodePostal);


                    int lignesModifiees = commande.ExecuteNonQuery();

                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("Le participant a bien été ajouté !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);


                        btnRetourPage1_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur BDD directe : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
