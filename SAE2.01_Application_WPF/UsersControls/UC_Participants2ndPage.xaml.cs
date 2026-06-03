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
            MessageBox.Show("Validation réussie !");
        }
    }
}
