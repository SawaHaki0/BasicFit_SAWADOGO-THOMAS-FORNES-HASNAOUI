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
    /// Logique d'interaction pour UC_StatsPage.xaml
    /// </summary>
    public partial class UC_StatsPage : UserControl
    {
        public UC_StatsPage()
        {
            InitializeComponent();
        }
        private void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "📅 Periode :")
            {
                box.Text = "";
                box.Foreground = Brushes.Black;
            }
        }

        // Gère la perte de focus du champ Période
        private void TextBox_NoFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                box.Foreground = Brushes.Gray;
                box.Text = "📅 Periode :";
            }
        }

        // Événement quand on clique sur n'importe quel bouton de catégorie
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            // Plus tard, tu filtreras tes statistiques en fonction de : btn.Content.ToString()
            MessageBox.Show($"Filtre appliqué : {btn.Content}");
        }
    }
}
