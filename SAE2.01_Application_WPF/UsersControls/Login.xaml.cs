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

            try
            {
                DataAccess da = new DataAccess(username, password);
                DataAccess.GetConnection();

                if (da.Role == "responsable_du_club")
                    OuvrirApplication(true);
                else
                    OuvrirApplication(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.",
                                 "Erreur de connexion",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
                DataAccess.CloseConnection();
            }

            
        }

        private void OuvrirApplication(bool estResponsable)
        {
            MainWindow fenetreSuivante = new MainWindow(estResponsable);
            Application.Current.MainWindow = fenetreSuivante;
            fenetreSuivante.Show();
            this.Window.Close();
        }
    }
}
