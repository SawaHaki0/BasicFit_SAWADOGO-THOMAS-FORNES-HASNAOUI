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
    /// Logique d'interaction pour UCCategorie.xaml
    /// </summary>
    public partial class UCCategorie : UserControl
    {
        public UCCategorie()
        {
            InitializeComponent();
        }

        private void butAjouterCate_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);

            if (fenetrePrincipale != null)
            {
                UCCategorieAjouter nouvellePage = new UCCategorieAjouter();
                fenetrePrincipale.MainContainer.Content = nouvellePage;
            }

        }

        private void butModifierCate_Click(object sender, RoutedEventArgs e)
        {
                
            if (dgSeances.SelectedItem is Categorie cate)
            {
                MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                if (fenetre != null)
                    fenetre.MainContainer.Content = new UCCategorieModifier(cate, this);
            }
            else
            {
                MessageBox.Show("Sélectionnez d'abord une séance dans le planning.",
                                "Aucune séance sélectionnée",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void butSupprimerCate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
