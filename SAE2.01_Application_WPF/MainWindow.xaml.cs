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
        UC_Participants1stPage uc = new UC_Participants1stPage();
        public MainWindow()
        {
            InitializeComponent();
            MainContainer.Children.Add(uc);
        }
    }
}