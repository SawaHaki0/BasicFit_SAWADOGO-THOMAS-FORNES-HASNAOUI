using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SAE2._01_Application_WPF.Classes;

namespace SAE2._01_Application_WPF.UsersControls
{
    /// <summary>
    /// Logique d'interaction pour UC_Séance.xaml
    /// </summary>
    public partial class UC_Séance : UserControl
    {
        public UC_Séance()
        {
            InitializeComponent();
        }
        private void FiltrerCategorie(object sender, RoutedEventArgs e)
        {
            string categorie = (sender as Button)?.Tag as string;

            ICollectionView vue = CollectionViewSource.GetDefaultView(dgSeances.ItemsSource);
            if (vue == null) return;

            if (string.IsNullOrEmpty(categorie))
                vue.Filter = null;   // "Toutes les catégories"
            else
                vue.Filter = obj =>
                    (obj as Seance)?.UnCours.UneCategorie.NomCategorie == categorie;

            MettreEnValeurBouton(sender as Button);   // surbrillance (optionnel)
        }
        private void MettreEnValeurBouton(Button actif)
        {
            BrushConverter bc = new BrushConverter();
            foreach (var child in panelCategories.Children)
            {
                if (child is Button b)
                {
                    b.Background = Brushes.White;
                    b.Foreground = (Brush)bc.ConvertFrom("#555555");
                    b.BorderThickness = new Thickness(1);
                }
            }
            if (actif != null)
            {
                actif.Background = (Brush)bc.ConvertFrom("#F26B1A");
                actif.Foreground = Brushes.White;
                actif.BorderThickness = new Thickness(0);
            }
        }
    }
}

