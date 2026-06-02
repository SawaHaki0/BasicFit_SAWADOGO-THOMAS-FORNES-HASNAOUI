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

namespace SAE2._01_Application_WPF
{
	/// <summary>
	/// Logique d'interaction pour Login.xaml
	/// </summary>
	public partial class Login : UserControl
	{
		public Login()
		{
			InitializeComponent();
		}

        private void butQuiter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void butConfirmer_Click(object sender, RoutedEventArgs e)
        {
            string username = textboxUsername.Text;
            string password = textBoxPassword.Password;   
            
            if (username == "admin" && password == "1234")
            {
                MainWindow fenetreSuivante = new MainWindow();
                fenetreSuivante.Show();
                Application.Current.Shutdown();          
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.",
                                "Erreur de connexion",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
