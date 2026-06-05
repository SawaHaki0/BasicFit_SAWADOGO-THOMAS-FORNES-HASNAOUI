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
    /// Logique d'interaction pour UC_SeanceModifier.xaml
    /// </summary>
    public partial class UC_SeanceModifier : UserControl
    {
        private Seance seance;
        private UserControl pagePrec;
        public UC_SeanceModifier(Seance seance, UserControl pagePrec)
        {
            InitializeComponent();
            this.Seance = seance;
            this.PagePrec = pagePrec;

            cbCours.ItemsSource = new Cours().FindAll();
            cbEntraineur.ItemsSource = new Entraineur().FindAll();
            cbSalle.ItemsSource = new Salle().FindAll();

            cbCours.SelectedItem = cbCours.Items.Cast<Cours>().FirstOrDefault(c => c.IdCours == seance.UnCours.IdCours);
            cbEntraineur.SelectedItem = cbEntraineur.Items.Cast<Entraineur>().FirstOrDefault(en => en.IdEntraineur == seance.UnEntraineur.IdEntraineur);
            cbSalle.SelectedItem = cbSalle.Items.Cast<Salle>().FirstOrDefault(s => s.IdSalle == seance.UneSalle.IdSalle);

            cbJour.SelectedIndex = seance.JourSeance;
            txtHeureDebut.Text = seance.HeureDebut.ToString("HH:mm");
            txtHeureFin.Text = seance.HeureFin.ToString("HH:mm");
            txtNbPlaces.Text = seance.NbPlaces.ToString();


        }

        public Seance Seance
        {
            get
            {
                return this.seance;
            }

            set
            {
                this.seance = value;
            }
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

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetre = (MainWindow)Window.GetWindow(this);
            if (fenetre != null)
            {
                fenetre.MainContainer.Content = new UCEntraineurs(); // Nom de votre UC liste
            }
        }

        private void btnFinaliserModification_Click(object sender, RoutedEventArgs e)
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

            int jour = cbJour.SelectedIndex;
            if (jour < 1 || jour > 7)
            {
                MessageBox.Show("Sélectionnez un jour valide.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // validation et conversion des champs simples

            if (!TimeOnly.TryParse(txtHeureDebut.Text.Trim(), out TimeOnly hd)
                || !TimeOnly.TryParse(txtHeureFin.Text.Trim(), out TimeOnly hf))
            {
                MessageBox.Show("Les heures doivent être au format HH:mm.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtNbPlaces.Text.Trim(), out int nbPlaces) || nbPlaces <= 0)
            {
                MessageBox.Show("Le nombre de places doit être un entier positif.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // affectation à l'objet séance
            this.Seance.UnCours = cours;
            this.Seance.UnEntraineur = entr;
            this.Seance.UneSalle = salle;
            this.Seance.JourSeance = jour;
            this.Seance.HeureDebut = hd;
            this.Seance.HeureFin = hf;
            this.Seance.NbPlaces = nbPlaces;

            // enregistrement
            int nb = this.Seance.Update();
            if (nb > 0)
            {
                MessageBox.Show("La séance a bien été modifiée !", "Succès",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                btnRetour_Click(sender, e);
            }
            else
            {
                MessageBox.Show("La modification a échoué.", "Erreur",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
