using SAE2._01_Application_WPF.Classes;
using SAE2._01_Application_WPF.UsersControls;
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
        private bool loggedInTantQueResponsable;

        // Constructeur par défaut : affiche le Login au démarrage
        public MainWindow()
        {
            Login uc = new Login(this);
            InitializeComponent();
            MainGrid.Children.Add(uc);
        }

        // Constructeur après connexion : true = responsable, false = employé
        public MainWindow(bool loggedInTantQueResponsable)
        {
            InitializeComponent();
            this.LoggedInTantQueResponsable = loggedInTantQueResponsable;

            // Menu selon le rôle
            if (loggedInTantQueResponsable)
            {
                MenuContainer.Content = new Interface_ResponsableDuClub();
            }
            else
            {
                MenuContainer.Content = new Interface_Employe();
            }

            // Page d'accueil commune aux deux rôles
            MainContainer.Content = new PlanningDuJour();
        }

        public bool LoggedInTantQueResponsable
        {
            get
            {
                return this.loggedInTantQueResponsable;
            }

            set
            {
                this.loggedInTantQueResponsable = value;
            }
        }
    }
}