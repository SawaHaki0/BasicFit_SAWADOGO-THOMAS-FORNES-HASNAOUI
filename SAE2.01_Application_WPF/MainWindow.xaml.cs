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

        public MainWindow()
        {
            Login uc = new Login(this);
            InitializeComponent();
            MainGrid.Children.Add(uc);
        }

        public MainWindow(bool loggedInTantQueResponsable)
        {
            InitializeComponent();
            this.LoggedInTantQueResponsable = loggedInTantQueResponsable;

            if (loggedInTantQueResponsable)
            {
                MenuContainer.Content = new Interface_ResponsableDuClub(this);
            }
            else
            {
                MenuContainer.Content = new Interface_Employe(this);
            }

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