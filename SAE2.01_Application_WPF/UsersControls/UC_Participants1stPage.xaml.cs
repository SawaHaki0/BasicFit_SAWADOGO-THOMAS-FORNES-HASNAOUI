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
using System.ComponentModel;


namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour UC_Participants1stPage.xaml
    /// </summary>
    public partial class UC_Participants1stPage : UserControl
    {
        public UC_Participants1stPage()
        {
            InitializeComponent();
        }
        public UC_Participants1stPage(int seanceId) : this()
        {
            dgListeParticipants.ItemsSource = new Client().FindBySeance(seanceId);
        }
        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "Recherche..." || box.Text == "Naissance..." || box.Text == "Adresse...")
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
                if (box == txtFiltreRecherche) box.Text = "Recherche...";
                if (box == txtFiltreNaissance) box.Text = "Naissance...";
                if (box == txtFiltreAdresse) box.Text = "Adresse...";
            }
        }

       

        private void btnOuvrirCreation_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {
                // 2. On instancie le deuxième UC
                UC_Participants2ndPage nouvellePage = new UC_Participants2ndPage();

                // CORRECTION ICI : On utilise MainContainer au lieu de MenuContainer
                fenetrePrincipale.MainContainer.Content = nouvellePage;
            }
        }
        private void Filtrer(object sender, TextChangedEventArgs e)
        {
            if (dgListeParticipants == null) return;

            ICollectionView vue = CollectionViewSource.GetDefaultView(dgListeParticipants.ItemsSource);
            if (vue == null) return;

            string recherche = TexteFiltre(txtFiltreRecherche, "Recherche...");
            string naissance = TexteFiltre(txtFiltreNaissance, "Naissance...");
            string adresse = TexteFiltre(txtFiltreAdresse, "Adresse...");

            vue.Filter = obj =>
            {
                if (!(obj is Client c)) return false;

                // Recherche : nom, prénom, mail ou téléphone
                bool okRecherche = recherche == ""
                    || (c.Nom ?? "").ToLower().Contains(recherche)
                    || (c.Prenom ?? "").ToLower().Contains(recherche)
                    || (c.Mail ?? "").ToLower().Contains(recherche)
                    || (c.Telephone ?? "").ToLower().Contains(recherche);

                // Naissance : tape "1990", "/05/", "12/05/1990"...
                bool okNaissance = naissance == ""
                    || c.DateNaissance.ToString("dd/MM/yyyy").Contains(naissance);

                // Adresse : adresse, ville ou code postal
                bool okAdresse = adresse == ""
                    || (c.Adresse ?? "").ToLower().Contains(adresse)
                    || (c.Ville ?? "").ToLower().Contains(adresse)
                    || (c.CodePostal ?? "").ToLower().Contains(adresse);

                return okRecherche && okNaissance && okAdresse;
            };
        }

        // Renvoie le texte saisi en minuscules, ou "" si c'est le placeholder
        private string TexteFiltre(TextBox box, string placeholder)
        {
            string t = box.Text.Trim();
            return (t == placeholder) ? "" : t.ToLower();
        }

    }
}
