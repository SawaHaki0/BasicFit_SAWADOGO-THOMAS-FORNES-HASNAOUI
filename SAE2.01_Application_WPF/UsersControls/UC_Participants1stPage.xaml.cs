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
            if (box.Text == "Recherche" || box.Text == "Naissance" || box.Text == "Adresse")
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
                if (box == txtFiltreRecherche) box.Text = "Recherche";
                if (box == txtFiltreNaissance) box.Text = "Naissance";
                if (box == txtFiltreAdresse) box.Text = "Adresse";
            }
        }

        private void btnOuvrirCreation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bouton opérationnel !");
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {
                // 2. On instancie le deuxième UC
                UC_Participants2ndPage nouvellePage = new UC_Participants2ndPage();

                // 3. On injecte le nouvel UC dans le ContentControl de la MainWindow
                fenetrePrincipale.MenuContainer.Content = nouvellePage;
            }

        }
    }
}
