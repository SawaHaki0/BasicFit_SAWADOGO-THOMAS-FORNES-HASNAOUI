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
        private MainWindow window;

        public MainWindow Window
        {
            get
            {
                return this.window;
            }

            set
            {
                this.window = value;
            }
        }

        public Login(MainWindow window)
        {
            InitializeComponent();
            this.Window = window;
        }

        private void butQuiter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void butConfirmer_Click(object sender, RoutedEventArgs e)
        {
            string username = textboxUsername.Text;
            string password = textBoxPassword.Password;   
            
            if (username == "responsable" && password == "1234")
            {
                MainWindow fenetreSuivante = new MainWindow(true);
                this.Window.Close();
                fenetreSuivante.Show();
                
            }
            else if (username == "employée" && password == "4321")
            {
                
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
