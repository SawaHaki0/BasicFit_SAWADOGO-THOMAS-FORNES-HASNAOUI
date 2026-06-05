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
    /// Logique d'interaction pour UC_SeanceAjouter.xaml
    /// </summary>
    public partial class UC_SeanceAjouter : UserControl
    {
        private UserControl pagePrec;
        public UC_SeanceAjouter(UserControl PagePrec)
        {
            InitializeComponent();
            this.PagePrec = pagePrec;

            cbCours.ItemsSource = new Cours().FindAll();
            cbEntraineur.ItemsSource = new Entraineur().FindAll();
            cbSalle.ItemsSource = new Salle().FindAll();
        }

        public UserControl PagePrec
        {
            get
            {
                return this.pagePrec;
            }

            set
            {
                this.pagePrec = value;
            }
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // validation des objets liés
            if (cbCours.SelectedItem is not Cours cours
                || cbEntraineur.SelectedItem is not Entraineur entr
                || cbSalle.SelectedItem is not Salle salle)
            {
                MessageBox.Show("Sélectionnez un cours, un entraîneur et une salle.",
                                "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // jour : Lundi est à l'index 0 → jour 1
            if (cbJour.SelectedIndex < 0)
            {
                MessageBox.Show("Sélectionnez un jour.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int jour = cbJour.SelectedIndex + 1;

            // heures
            if (!TimeOnly.TryParse(txtHeureDebut.Text.Trim(), out TimeOnly hd)
                || !TimeOnly.TryParse(txtHeureFin.Text.Trim(), out TimeOnly hf))
            {
                MessageBox.Show("Les heures doivent être au format HH:mm.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // nombre de places
            if (!int.TryParse(txtNbPlaces.Text.Trim(), out int nbPlaces) || nbPlaces <= 0)
            {
                MessageBox.Show("Le nombre de places doit être un entier positif.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // construction de la nouvelle séance
            Seance nouvelle = new Seance
            {
                UnCours = cours,
                UnEntraineur = entr,
                UneSalle = salle,
                JourSeance = jour,
                HeureDebut = hd,
                HeureFin = hf,
                NbPlaces = nbPlaces
            };

            int nb = nouvelle.Create();
            if (nb > 0)
            {
                MessageBox.Show("La séance a bien été ajoutée !", "Succès",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                btnRetour_Click(sender, e);
            }
            else
            {
                MessageBox.Show("L'ajout a échoué.", "Erreur",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                fenetre.MainContainer.Content = new UCEntraineurs(); 
            }
        }
    }
}
