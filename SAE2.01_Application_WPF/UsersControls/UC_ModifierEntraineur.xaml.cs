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
    /// Logique d'interaction pour UC_ModifierEntraineur.xaml
    /// </summary>
    public partial class UC_ModifierEntraineur : UserControl
    {
        private int idEntraineurAModifier;

        public UC_ModifierEntraineur()
        {
            InitializeComponent();

        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                fenetre.MenuContainer.Content = new UCEntraineurs(); // Nom de votre UC liste
            }
        }

        private void btnFinaliserModification_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtSaisieNom.Text.Trim();
            string prenom = txtSaisiePrenom.Text.Trim();

            // Validation de sécurité
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom))
            {
                MessageBox.Show("Le Nom et le Prénom ne peuvent pas être vides !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Requête SQL UPDATE filtrée par l'ID de l'entraîneur
            string requete = "UPDATE Entraineur SET nom = @nom, prenom = @prenom WHERE id_entraineur = @id;";

            try
            {
                var connexion = DataAccess.GetConnection();

                NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@id", idEntraineurAModifier);

                int lignesModifiees = commande.ExecuteNonQuery();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("L'entraîneur a bien été mis à jour !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Retour automatique à la liste
                    btnRetour_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification : " + ex.Message, "Erreur BDD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
