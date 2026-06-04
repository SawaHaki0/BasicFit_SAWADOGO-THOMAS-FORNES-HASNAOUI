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

            Categorie newCate = new Categorie(nom, description);

                if (string.IsNullOrEmpty(nom) || nom == "Nom de la catégorie" || string.IsNullOrEmpty(description) || description == "Description de la catégorie")
                {
                    MessageBox.Show("Le Nom et la Description de la catégorie sont obligatoires !", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            }
            Collections.Categories.Add(newCate);
            MessageBox.Show("La catégorie a bien été ajoutée à la base de données.", "Ajout réussi", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow fenetrePrincipale = (MainWindow)Window.GetWindow(this);
        }
    }
}
