using SAE2._01_Application_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour UCCategorieAjouter.xaml
    /// </summary>
    public partial class UCCategorieAjouter : UserControl
    {
        public UCCategorieAjouter()
        {
            InitializeComponent();
        }

        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {
                UCCategorie nouvellePage = new UCCategorie();
                fenetrePrincipale.MainContainer.Content = nouvellePage;
            }

        }

        private void butAjouter_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtNomCate.Text.Trim();
            string description = txtDescCate.Text.Trim();
            if (string.IsNullOrWhiteSpace(nom) ||
        string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Le Nom et la Description de la catégorie sont obligatoires !");
                return;
            }
        }
    }
}
