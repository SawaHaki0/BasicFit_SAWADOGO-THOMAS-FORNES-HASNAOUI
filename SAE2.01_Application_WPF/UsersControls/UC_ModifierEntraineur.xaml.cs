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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour UC_ModifierEntraineur.xaml
    /// </summary>
    public partial class UC_ModifierEntraineur : UserControl
    {
        private UserControl pagePrecedente;
        private Entraineur trainer;

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

        public Entraineur Trainer
        {
            get
            {
                return this.trainer;
            }

            set
            {
                this.trainer = value;
            }
        }

        public UC_ModifierEntraineur(Entraineur trainer, UserControl pagePrec)
        {
            InitializeComponent();
            this.PagePrecedente = pagePrec;
            this.Trainer = trainer;

            txtSaisieNom.Text = trainer.NomEntraineur;
            txtSaisiePrenom.Text = trainer.PrenomEntraineur;

        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                fenetre.MainContainer.Content = new UCEntraineurs(); 
            }
        }

        private void btnFinaliserModification_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtSaisieNom.Text.Trim();
            string prenom = txtSaisiePrenom.Text.Trim();


            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom))
            {
                MessageBox.Show("Le Nom et le Prénom ne peuvent pas être vides !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string requete = "UPDATE Entraineur SET entraineur_nom = @nom, entraineur_prenom = @prenom WHERE entraineur_id = @id;";
            try
            {
                var connexion = DataAccess.GetConnection();

                NpgsqlCommand commande = new NpgsqlCommand(requete, connexion);
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@id", this.Trainer.IdEntraineur);
                int lignesModifiees = commande.ExecuteNonQuery();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("L'entraîneur a bien été mis à jour !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                    if (fenetre != null)
                    {
                        fenetre.MainContainer.Content = new UCEntraineurs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification : " + ex.Message, "Erreur BDD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
