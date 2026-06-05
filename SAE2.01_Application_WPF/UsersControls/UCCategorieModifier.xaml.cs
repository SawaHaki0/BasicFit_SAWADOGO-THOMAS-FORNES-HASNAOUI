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
    /// Logique d'interaction pour UCCategorieModifier.xaml
    /// </summary>
    public partial class UCCategorieModifier : UserControl
    {
        private UserControl pagePrecedente;
        private Categorie cate;

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
        
        public Categorie Cate
        {
            get
            {
                return this.cate;
            }

            set
            {
                this.cate = value;
            }
        }

        public UCCategorieModifier(Categorie cate, UserControl pagePrec)
        {
            InitializeComponent();
            this.Cate = cate;
            this.PagePrecedente = pagePrec;

            
            txtNomCate.Text = cate.NomCategorie;
            txtDescCate.Text = cate.DescriptionCategorie;
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

        private void butModifierCate_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtNomCate.Text.Trim();
            string description = txtDescCate.Text.Trim();

                if (string.IsNullOrEmpty(nom) || nom == "Nom de la catégorie" || string.IsNullOrEmpty(description) || description == "Description de la catégorie")
                {
                    MessageBox.Show("Le Nom et la Description de la catégorie sont obligatoires !", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            
                }

            this.Cate.NomCategorie = nom;
            this.Cate.DescriptionCategorie = description;

            int nb = this.Cate.Update();
            if (nb > 0)
            {
                MessageBox.Show("Catégorie modifiée avec succès !", "Succès",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow fenetre = (MainWindow)Window.GetWindow(this);
                if (fenetre != null)
                    fenetre.MainContainer.Content = new UCCategorie();
            }
            else
            {
                MessageBox.Show("La modification a échoué.", "Erreur",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
