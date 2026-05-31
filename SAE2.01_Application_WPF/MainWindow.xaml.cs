using SAE2._01_Application_WPF.Classes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE2._01_Application_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Interface_Employe uc = new Interface_Employe();
        public MainWindow()
        {
            InitializeComponent();
            MainContainer.Children.Add(uc);

            if (DataAccess.TestConnection())
            {
                MessageBox.Show("Connexion réussie !", "DB", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Impossible de se connecter à la base de données.\nVérifiez votre VPN.",
                                "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); // optional: close app if no DB
            }
        }
    }
}